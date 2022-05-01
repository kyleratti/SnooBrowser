using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

namespace SnooBrowser.Util
{
    public static class Policies
    {
        private static AsyncTimeoutPolicy<HttpResponseMessage> TimeoutPolicy =>
            Policy.TimeoutAsync<HttpResponseMessage>(seconds: 15, (context, timeSpan, task) => Task.CompletedTask);

        private static bool IsAuthFailure(HttpRequestMessage request, HttpStatusCode statusCode)
        {
            // Only these status codes can be the result of auth failure, so if this status code isn't one we can skip the checks below.
            if (statusCode is not (
                HttpStatusCode.Unauthorized
                or HttpStatusCode.Forbidden
                or HttpStatusCode.NotFound
            ))
                return false;

            // Unauthorized == 401
            // This should always be a token issue
            if (statusCode is HttpStatusCode.Unauthorized)
                return true;

            // There are several endpoints that return 403 Forbidden or 404 Not Found when called with an invalid access token.
            // This is wrong.
            // 403 Forbidden means "you're authenticated, but you can't access this, so don't try again".
            // 404 Not Found means "this doesn't exist, authenticated or not".
            // I think it's worse to always treat 403 and 404 as 401, so I'd rather add exceptions for endpoints as discovered.
            // The real fix is for reddit to follow the damn HTTP spec.
            var endpointsThatReturnWrongStatusCode = new[]
            {
                "reddit.com/user/",
                "/modqueue.json"
            };

            var uri = request.RequestUri;

            return statusCode is HttpStatusCode.Forbidden or HttpStatusCode.NotFound &&
                   endpointsThatReturnWrongStatusCode.Any(s => uri!.AbsolutePath.Contains(s, StringComparison.OrdinalIgnoreCase));
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(ISnooBrowser browser) =>
            Policy
                .HandleResult<HttpResponseMessage>(res =>
                    res.StatusCode is HttpStatusCode.Unauthorized
                        or HttpStatusCode.Forbidden
                        or HttpStatusCode.NotFound
                )
                .RetryAsync(
                    retryCount: 1,
                    onRetryAsync: async (delegateResult, retryCount) =>
                    {
                        var result = delegateResult.Result;
                        if (IsAuthFailure(result.RequestMessage!, result.StatusCode))
                            await browser.RefreshAccessToken();
                    }
                );

        public static AsyncPolicyWrap<HttpResponseMessage> GetPolicyStrategy(ISnooBrowser browser) =>
            Policy.WrapAsync(GetRetryPolicy(browser), TimeoutPolicy);
    }
}
