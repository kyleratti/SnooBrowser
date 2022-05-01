using System.Threading.Tasks;
using SnooBrowser.Structures.Subreddit;

namespace SnooBrowser.Interfaces.Scopes
{
    public interface ISubreddit
    {
        public Task<GetAboutSubredditResponse> GetAboutSubreddit(string subreddit);
    }
}
