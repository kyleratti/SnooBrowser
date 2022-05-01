using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnooBrowser.Interfaces.Monitors;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Structures.SubredditModeration;
using SnooBrowser.Util;

namespace SnooBrowser.Monitors
{
    public class ModqueuePollingConsumer : PollingConsumer
    {
        private readonly ISubredditModeration _subredditModeration;
        private readonly CancellationToken _cancellationToken;
        private readonly Dictionary<string, int> _seenReports;

        private ModqueuePollingConsumer(ISubredditModeration subredditModeration, TimeSpan interval, string subreddit,
            CancellationToken cancellationToken)
            : base(interval)
        {
            _subredditModeration = subredditModeration;
            Subreddit = subreddit;
            _cancellationToken = cancellationToken;
            _seenReports = new Dictionary<string, int>(capacity: 500);
        }

        public static ModqueuePollingConsumer Create(ISubredditModeration subredditModeration, TimeSpan interval, string subreddit, 
            CancellationToken cancellationToken) =>
            new(subredditModeration, interval, subreddit, cancellationToken);

        public string Subreddit { get; }

        public event EventHandler<ModQueueEntryData>? NewModqueueItem;

        private async IAsyncEnumerable<ModQueueEntryData> GetModqueueItems()
        {
            var resp = await _subredditModeration.GetModQueue(Subreddit);

            foreach (var entry in resp.Data.Entries)
            {
                yield return entry.Data;
            }
        }

        public override async Task Tick()
        {
            var modqueueItems = GetModqueueItems().WithCancellation(_cancellationToken);

            await foreach (var item in modqueueItems)
            {
                var totalReportCount = item.UserReports.Length + item.ModReports.Length;

                if (_seenReports.ContainsKey(item.NameAsString) && _seenReports[item.NameAsString] == totalReportCount)
                    continue;

                _seenReports[item.NameAsString] = totalReportCount;
                NewModqueueItem?.Invoke(this, item);
            }
        }
    }

    public class ModqueueMonitor : IModqueueMonitor
    {
        private readonly ISubredditModeration _subredditModeration;

        public ModqueueMonitor(ISubredditModeration subredditModeration)
        {
            _subredditModeration = subredditModeration;
        }

        public ModqueuePollingConsumer Monitor(string subreddit, CancellationToken cancellationToken) =>
            ModqueuePollingConsumer.Create(
                _subredditModeration,
                TimeSpan.FromSeconds(45),
                subreddit,
                cancellationToken
            );
    }
}
