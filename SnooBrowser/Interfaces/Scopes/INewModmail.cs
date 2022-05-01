using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Interfaces.Monitors;
using SnooBrowser.Structures;
using SnooBrowser.Structures.NewModmail;

namespace SnooBrowser.Interfaces.Scopes
{
    public interface INewModmail
    {
        public Task<GetConversationsResponse> GetConversations(string subreddit, ModmailSort sort,
            ModmailState state, int limit = 25, Maybe<string> after = default);
        public Task<GetConversationsResponse> GetConversations(string[] subreddits,
            ModmailSort sort, ModmailState state, int limit = 25, Maybe<string> after = default);

        public Task<bool> ArchiveConversation(Things.ConversationId convoId);

        public Task<GetConversationResponse> GetConversation(Things.ConversationId convoId);

        public IModmailMonitor CreateMonitor();
    }
}
