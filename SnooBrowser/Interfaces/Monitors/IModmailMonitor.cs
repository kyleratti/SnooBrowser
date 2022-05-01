using System.Threading;
using FruityFoundation.Base.Structures;
using SnooBrowser.Monitors;
using SnooBrowser.Structures;

namespace SnooBrowser.Interfaces.Monitors
{
    public interface IModmailMonitor
    {
        public NewModmailPollingConsumer MonitorNew(string subreddit, ModmailState state, int limit, CancellationToken cancellationToken, Maybe<string> after = default);
    }
}
