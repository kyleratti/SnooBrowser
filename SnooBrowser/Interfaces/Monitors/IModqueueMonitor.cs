using System.Threading;
using SnooBrowser.Monitors;

namespace SnooBrowser.Interfaces.Monitors
{
    public interface IModqueueMonitor
    {
        public ModqueuePollingConsumer Monitor(string subreddit, CancellationToken cancellationToken);
    }
}
