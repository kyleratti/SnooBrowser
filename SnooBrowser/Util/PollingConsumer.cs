using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnooBrowser.Util
{
    public abstract class PollingConsumer : IPollingConsumer
    {
        protected PollingConsumer(TimeSpan interval)
        {
            TickInterval = interval;

            Initialize();
        }
        
        protected TimeSpan TickInterval { get; }
        protected CancellationTokenSource CancellationTokenSource { get; private set; } = new();

        private void Initialize()
        {
            CancellationTokenSource = new CancellationTokenSource();
        }

        public virtual async Task StartAsync()
        {
            Stop();
            Initialize();
            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    OnPreTick?.Invoke(this, EventArgs.Empty);
                    await Tick();
                }
                catch (Exception ex)
                {
                    // Throw as usual if there are no exception handlers
                    if (OnException == null)
                        throw;

                    OnException.Invoke(this, ex);
                }

                await Task.Delay(TickInterval, CancellationTokenSource.Token);
            }
        }

        public event EventHandler<Exception>? OnException;
        public event EventHandler? OnPreTick;


        protected void Stop()
        {
            CancellationTokenSource.Cancel();
        }

        public abstract Task Tick();
    }
}
