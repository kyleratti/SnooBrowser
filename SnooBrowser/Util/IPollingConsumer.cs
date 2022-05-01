using System;
using System.Threading.Tasks;

namespace SnooBrowser.Util
{
    public interface IPollingConsumer
    {
        public Task Tick();
        public Task StartAsync();
        public event EventHandler<Exception> OnException;
        public event EventHandler OnPreTick;
    }
}
