using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Interfaces.Monitors;
using SnooBrowser.Structures.SubredditModeration;
using SnooBrowser.Things;

namespace SnooBrowser.Interfaces.Scopes
{
    public interface ISubredditModeration
    {
        public Task<GetModLogResponse> GetModLog(string subreddit, int limit = 100);
        public Task<bool> RemoveContent(Fullname fullname);
        public Task<bool> ApproveContent(Fullname fullname);
        public Task<bool> IgnoreReports(Fullname fullname);
        public Task<bool> SpamContent(Fullname fullname);
        public Task BanUser(string username, (Fullname Fullname, string DisplayName) subreddit, Maybe<int> duration, string modNote, string userMessage);
        //public Task<bool> GetAuditLog()
        public Task<GetModQueueResponse> GetModQueue(string subreddit, int limit = 100);
        public IModqueueMonitor CreateModqueueMonitor();
    }
}
