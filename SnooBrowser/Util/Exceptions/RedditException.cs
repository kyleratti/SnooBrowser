using System;
using System.Net;

namespace SnooBrowser.Util.Exceptions
{
    public class RedditException : Exception
    {
        public RedditException(HttpStatusCode statusCode, string body)
        {
            StatusCode = statusCode;
            Contents = body;
        }
        
        public HttpStatusCode StatusCode { get; init; }
        public string Contents { get; init; }
    }
}
