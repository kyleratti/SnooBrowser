using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Interfaces.Monitors;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Structures;
using SnooBrowser.Structures.NewModmail;
using SnooBrowser.Util;

namespace SnooBrowser.Monitors
{
    public class NewModmailPollingConsumer : PollingConsumer
    {
        private readonly INewModmail _newModmail;
        private readonly HashSet<string> _seenMessages;
        private readonly int _limit;
        private readonly CancellationToken _cancellationToken;

        private NewModmailPollingConsumer(INewModmail newModmail, TimeSpan interval, string[] subreddits, int limit, CancellationToken cancellationToken)
            : base(interval)
        {
            _newModmail = newModmail;
            _seenMessages = new HashSet<string>(capacity: 500);
            _limit = limit;
            _cancellationToken = cancellationToken;
            Subreddits = subreddits;
        }

        public static NewModmailPollingConsumer Create(INewModmail newModmail, TimeSpan interval, string[] subreddits, int limit, CancellationToken cancellationToken) =>
            new(newModmail, interval, subreddits, limit, cancellationToken);

        public static NewModmailPollingConsumer Create(INewModmail newModmail, TimeSpan interval, string subreddit, int limit, CancellationToken cancellationToken) =>
            Create(newModmail, interval, new[] { subreddit }, limit, cancellationToken);

        private string[] Subreddits { get; }

        private async IAsyncEnumerable<(Conversation Conversation, ConversationMessage Message)> GetNewConversations()
        {
            var resp = await _newModmail.GetConversations(
                subreddits: Subreddits,
                sort: ModmailSort.Recent,
                state: ModmailState.All,
                limit: _limit);

            foreach (var (msgId, msg) in resp.Messages)
            {
                if (_seenMessages.Contains(msg.Id))
                    continue;

                _seenMessages.Add(msg.Id); // FIXME: timestamps, auto-purge

                yield return (
                    Conversation: resp
                        .Conversations
                        .First(convo => convo.Value.ObjectIds.Any(obj => obj.Id == msgId))
                        .Value,
                    Message: msg
                );
            }
        }

        public override async Task Tick()
        {
            var convos = GetNewConversations().WithCancellation(_cancellationToken);

            await foreach (var convoSet in convos)
            {
                OnNewMessage?.Invoke(this, convoSet);
            }
        }

        public event EventHandler<(Conversation Conversation, ConversationMessage Message)>? OnNewMessage;
    }
    
    public class ModmailMonitor : IModmailMonitor
    {
        private readonly INewModmail _newModmail;

        internal ModmailMonitor(INewModmail newModmail)
        {
            _newModmail = newModmail;
        }

        public NewModmailPollingConsumer MonitorNew(string subreddit, ModmailState state, int limit,
            CancellationToken cancellationToken, Maybe<string> after = default) =>
            NewModmailPollingConsumer.Create(
                _newModmail,
                TimeSpan.FromSeconds(15),
                subreddit,
                limit,
                cancellationToken
            );
    }
}
