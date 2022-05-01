using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SnooBrowser.Util
{
    public class PolicyHandler : DelegatingHandler
    {
        public PolicyHandler(ISnooBrowser browser)
        {
            Browser = browser;
        }
        
        private ISnooBrowser Browser { get; init; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage req,
            CancellationToken cancellationToken) =>
            Policies
                .GetPolicyStrategy(Browser)
                .ExecuteAsync(ct => base.SendAsync(req, ct), cancellationToken);
    }
}
