using System.Threading.Tasks;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Structures;

namespace SnooBrowser
{
    public interface ISnooBrowser : ISnooBrowserRequestHandler
    {
        public AuthParameters AuthParameters { get; }
        public INewModmail NewModmail { get; }
        public ISubredditModeration SubredditModeration { get; }
        public IMe Me { get; }
        public IUser User { get; }
        public ISubreddit Subreddit {get; }
        public Task RefreshAccessToken();
    }
}
