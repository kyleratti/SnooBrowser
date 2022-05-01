using System;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using FruityFoundation.Base.Structures;
using Newtonsoft.Json;
using SnooBrowser.Interfaces.Monitors;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Monitors;
using SnooBrowser.Structures;
using SnooBrowser.Structures.SubredditModeration;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Scopes
{
    public class SubredditModeration : BaseBrowserConsumer, ISubredditModeration
    {
        private readonly IUser _user;
        private readonly ISubreddit _subreddit;

        public SubredditModeration(ISnooBrowser browser)
            : base(browser)
        {
            _user = new User(browser);
            _subreddit = new Subreddit(browser);
        }

        public Task<GetModLogResponse> GetModLog(string subreddit, int limit = 100) =>
            _browser
                .CreateRequest("r/")
                .AppendPathSegment(subreddit)
                .AppendPathSegment("/about/log.json")
                .SetQueryParam("limit", limit)
                .GetJsonAsync<GetModLogResponse>();

        public Task<GetModQueueResponse> GetModQueue(string subreddit, int limit = 100) =>
            _browser
                .CreateRequest("r/")
                .AppendPathSegment(subreddit)
                .AppendPathSegment("/about/modqueue.json")
                .SetQueryParam("limit", limit)
                .GetJsonAsync<GetModQueueResponse>();

        private async Task<bool> RemoveContent(Fullname fullname, bool isSpam) =>
            (
                await _browser
                .CreateRequest("api/remove")
                .PostUrlEncodedAsync(new
                {
                    id = fullname.FullId,
                    spam = isSpam
                })
            ).StatusCode == (int)HttpStatusCode.OK;

        public Task<bool> RemoveContent(Fullname fullname) =>
            RemoveContent(fullname, isSpam: false);

        public async Task<bool> ApproveContent(Fullname fullname) =>
            (await _browser
                .CreateRequest("api/approve")
                .PostUrlEncodedAsync(new
                {
                    id = fullname.FullId
                })
            ).StatusCode == (int)HttpStatusCode.OK;

        public async Task<bool> IgnoreReports(Fullname fullname) =>
            (await _browser
                .CreateRequest("api/ignore_reports")
                .PostUrlEncodedAsync(new
                {
                    id = fullname.FullId
                })
            ).StatusCode == (int)HttpStatusCode.OK;

        public Task<bool> SpamContent(Fullname fullname) =>
            RemoveContent(fullname, isSpam: true);

        public async Task BanUser(string username, (Fullname Fullname, string DisplayName) subreddit, Maybe<int> duration, string modNote, string userMessage)
        {
            if (duration.Try(out var banLength))
            {
                switch (banLength)
                {
                    case <= 0:
                        throw new ArgumentOutOfRangeException(nameof(duration), banLength, "Argument cannot be <= 0. If you want to submit a permanent ban, pass an Empty.");
                    case > 999:
                        throw new ArgumentOutOfRangeException(nameof(duration), banLength,
                            "Argument cannot be > 999. If you want to submit a permanent ban, pass an Empty.");
                }
            }

            var resp =
                await _browser.CreateRequest("r/")
                    .AppendPathSegment(subreddit.DisplayName)
                    .AppendPathSegment("/api/friend")
                    .PostUrlEncodedAsync(new
                    {
                        action = "add",
                        container = subreddit.Fullname.FullId,
                        type = FriendActionType.Banned.ToApiName(),
                        name = username,
                        ban_reason = "", // If set to anything, this prefixes the note property below.
                        note = modNote,
                        ban_message = userMessage,
                        id = "#banned",
                        ban_context =
                            "", // This is seemingly unused, and although the docs say it should be a Thing's full ID, it doesn't.
                        duration = duration.ToNullable(),
                        r = subreddit.DisplayName,
                        permission = ""
                    });

            var rawBody = await resp.GetStringAsync();
            var uiResponse = JsonConvert.DeserializeObject<RedditUiResponse>(rawBody);

            if (resp.StatusCode != (int)HttpStatusCode.OK || !(uiResponse?.IsSuccess ?? false))
                throw new Exception($"Failed to ban user: {rawBody}");
        }
        
        public IModqueueMonitor CreateModqueueMonitor() =>
            new ModqueueMonitor(this);
    }
}
