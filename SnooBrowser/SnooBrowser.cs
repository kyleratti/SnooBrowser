using System;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using SnooBrowser.Interfaces.Auth;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Scopes;
using SnooBrowser.Structures;
using SnooBrowser.Util;

namespace SnooBrowser
{
    public sealed class SnooBrowser : ISnooBrowser
    {
        private const string _userAgent = "SnooBrowser v1.0 (by /u/rylekatti)"; // FIXME: get version # automatically

        private SnooBrowser(string appId, string appSecret, string refreshToken)
        {
            // Always create with a dummy access token. The client will handle getting a new one when we get a 401 Unauthorized
            // TODO: store last access token and try to load
            RestClient = CreateRestClient(new AccessToken(Token: "", DateTime.Now));
            //_ = InitializeClient();
            AuthParameters = new AuthParameters(
                appId,
                appSecret,
                refreshToken
            );
            NewModmail = new NewModmail(this);
            SubredditModeration = new SubredditModeration(this);
            Me = new Me(this);
            User = new User(this);
            Subreddit = new Subreddit(this);
        }

        private async Task InitializeClient()
        {
            RestClient = CreateRestClient(await GetNewAccessToken());
        }

        public static SnooBrowser Create(string appId, string appSecret, string refreshToken)
        {
            var browser = new SnooBrowser(appId, appSecret, refreshToken);
            return browser;
        }

        /*private AsyncRetryPolicy<T> UseTokenRefreshPolicy<T>() =>
            Policy
                .HandleResult<HttpResponseMessage>(resp => resp.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(
                    retryCount: 1,
                    onRetryAsync: (ex, retryCount, context) =>
                    {
                        if (ex.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            // TODO: log this
                            await RefreshAccessToken();
                        }

                        return resp;
                    }
                );*/

        private IFlurlClient CreateRestClient(AccessToken accessToken)
        {
            var client = CreateGenericRestClient("https://oauth.reddit.com/");
            client.Configure(settings =>
            {
                settings.HttpClientFactory = new PollyHttpClientFactory(this);
                // New reddit APIs use camel case
                settings.JsonSerializer = new NewtonsoftJsonSerializer(Serializer.CamelCaseJsonSerializerSettings);
            });
            client.WithOAuthBearerToken(accessToken.Token);
            return client;
        }

        private static IFlurlClient CreateGenericRestClient(string baseUrl)
        {
            var client = new FlurlClient(baseUrl)
            {
                Settings =
                {
                    // Old reddit APIs and the auth API use snake case
                    JsonSerializer = new NewtonsoftJsonSerializer(Serializer.SnakeCaseJsonSerializerSettings)
                }
            };
            client.Headers.Add("User-Agent", _userAgent);
            return client;
        }

        private IFlurlClient RestClient { get; set; }

        public AuthParameters AuthParameters { get; private init; }

        public INewModmail NewModmail { get; private init; }

        public ISubredditModeration SubredditModeration { get; private init; }
        
        public IMe Me { get; private init; }

        public IUser User { get; private init; }

        public ISubreddit Subreddit { get; private init; }

        private async Task<AccessToken> GetNewAccessToken()
        {
            // Use a generic REST client so this doesn't start an infinite loop of RefreshAccessToken calls
            var accessToken = await CreateGenericRestClient("https://www.reddit.com/")
                .WithBasicAuth(username: AuthParameters.AppId, password: AuthParameters.AppSecret)
                .Request("api/v1/access_token")
                .PostUrlEncodedAsync(new
                {
                    grant_type = "refresh_token",
                    refresh_token = AuthParameters.RefreshToken
                })
                .ReceiveJson<GetAccessTokenResponse>();

            return new AccessToken(
                Token: accessToken.AccessToken,
                ExpiresAt: DateTime.Now.AddSeconds(accessToken.ExpiresIn - (60 * 2)) // Reddit issues tokens for 1 hour, but let's do 58 minutes - just to be safe
            );
        }

        public async Task RefreshAccessToken()
        {
            RestClient = CreateRestClient(await GetNewAccessToken());
        }

        public IFlurlRequest CreateRequest(string url) =>
            RestClient.Request(url);

        /*private async Task<IRestResponse<T>> ExecuteAsyncImpl<T>(IRestRequest req, bool throwOnUnauthorized = false)
        {
            try
            {
                if (!RestClient.HasValue)
                    await RefreshAccessToken();

                var resp = await RestClient.Value.ExecuteAsync<T>(req);

                switch (resp.StatusCode)
                {
                    // 401 Unauthorized
                    // This will be the status code if your access_token is expired
                    // See more: https://github.com/reddit-archive/reddit/wiki/OAuth2
                    case HttpStatusCode.Unauthorized:
                        if (throwOnUnauthorized)
                            throw new RedditUnauthorizedException(HttpStatusCode.Unauthorized, resp.Content);
                        
                        // TODO: get new access_token
                        await RefreshAccessToken();
                        // Try one more time, but this time throw if we get a 401 Unauthorized again.
                        // If we get a 401 Unauthorized code again, something is wrong with our access token (check your scope)
                        return await ExecuteAsyncImpl<T>(req, throwOnUnauthorized: true);
                    default:
                        return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }*/
    }
}
