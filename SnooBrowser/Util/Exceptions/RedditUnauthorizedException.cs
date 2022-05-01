using System.Net;

namespace SnooBrowser.Util.Exceptions
{
    public class RedditUnauthorizedException : RedditException
    {
        public RedditUnauthorizedException(HttpStatusCode statusCode, string body)
        : base(statusCode, body)
        { }
    }
}
