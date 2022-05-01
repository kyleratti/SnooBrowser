using Flurl.Http;

namespace SnooBrowser
{
    public interface ISnooBrowserRequestHandler
    {
        public IFlurlRequest CreateRequest(string url);
    }
}
