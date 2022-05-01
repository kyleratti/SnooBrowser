using System.Net.Http;
using Flurl.Http.Configuration;

namespace SnooBrowser.Util
{
    public class PollyHttpClientFactory : DefaultHttpClientFactory
    {
        public PollyHttpClientFactory(ISnooBrowser browser)
        {
            Browser = browser;
        }
        
        private ISnooBrowser Browser { get; init; }

        public override HttpMessageHandler CreateMessageHandler() =>
            new PolicyHandler(Browser)
            {
                InnerHandler = base.CreateMessageHandler()
            };
    }
}
