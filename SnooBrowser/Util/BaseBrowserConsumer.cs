namespace SnooBrowser.Util
{
    public class BaseBrowserConsumer
    {
        protected readonly ISnooBrowser _browser;

        protected BaseBrowserConsumer(ISnooBrowser browser)
        {
            _browser = browser;
        }
    }
}
