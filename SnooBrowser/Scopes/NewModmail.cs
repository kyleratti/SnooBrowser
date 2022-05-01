using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using FruityFoundation.Base.Structures;
using SnooBrowser.Interfaces.Monitors;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Monitors;
using SnooBrowser.Structures;
using SnooBrowser.Structures.NewModmail;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Scopes
{
    public class NewModmail : BaseBrowserConsumer, INewModmail
    {
        public NewModmail(ISnooBrowser browser)
            : base(browser)
        { }

        public Task<GetConversationsResponse> GetConversations(string subreddit, ModmailSort sort,
            ModmailState state, int limit = 25, Maybe<string> after = default) =>
            GetConversations(new[] { subreddit }, sort, state, limit, after);

        public async Task<GetConversationsResponse> GetConversations(string[] subreddits, ModmailSort sort, ModmailState state, int limit = 25, Maybe<string> after = default)
        {
            var req = _browser.CreateRequest("api/mod/conversations");
            req.SetQueryParams(new
            {
                entity = string.Join(',', subreddits),
                limit,
                sort = sort.ToName(),
                state = state.ToName()
            });
            if (after.Try(out var str))
                req.SetQueryParam("after", str);

            return await req.GetJsonAsync<GetConversationsResponse>();
        }

        public async Task<bool> ArchiveConversation(ConversationId convoId) =>
            (await _browser.CreateRequest("api/mod/conversations")
                .AppendPathSegment(convoId.Id)
                .AppendPathSegment("archive")
                .PostAsync()).StatusCode == (int) HttpStatusCode.OK;

        public Task<GetConversationResponse> GetConversation(ConversationId convoId) =>
            _browser
                .CreateRequest("api/mod/conversations/")
                .AppendPathSegment(convoId.Id)
                .GetJsonAsync<GetConversationResponse>();

        public IModmailMonitor CreateMonitor() =>
            new ModmailMonitor(this);
    }
}
