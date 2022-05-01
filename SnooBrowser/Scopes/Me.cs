using System.Threading.Tasks;
using Flurl.Http;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Structures.Me;
using SnooBrowser.Util;

namespace SnooBrowser.Scopes
{
    public class Me : BaseBrowserConsumer, IMe
    {
        public Me(ISnooBrowser browser)
            : base(browser)
        { }

        public Task<GetMeResponse> GetMe() =>
            _browser
                .CreateRequest("api/v1/me")
                .GetJsonAsync<GetMeResponse>();
    }
}
