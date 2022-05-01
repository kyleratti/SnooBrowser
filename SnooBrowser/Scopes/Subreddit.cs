using System.Threading.Tasks;
using Flurl.Http;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Structures.Subreddit;
using SnooBrowser.Util;

namespace SnooBrowser.Scopes
{
    public class Subreddit : BaseBrowserConsumer, ISubreddit
    {
        public Subreddit(ISnooBrowser browser)
            : base(browser)
        { }

        public async Task<GetAboutSubredditResponse> GetAboutSubreddit(string subreddit) =>
            await _browser.CreateRequest("r/")
                .AppendPathSegment(subreddit)
                .AppendPathSegment("/about.json")
                .GetJsonAsync<GetAboutSubredditResponse>();
    }
}
