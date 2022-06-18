using System.Net.Http;

namespace SnooBrowser.Util;

public abstract class HttpResponseType
{
}

public class SuccessResponseType<T> : HttpResponseType
{
	public HttpResponseMessage Response { get; }
	public T Value { get; }

	public SuccessResponseType(HttpResponseMessage response, T value)
	{
		Response = response;
		Value = value;
	}
}

public class ErrorResponseType : HttpResponseType
{
	public HttpResponseMessage Response { get; }
	public string RawBody { get; }

	public ErrorResponseType(HttpResponseMessage response, string rawBody)
	{
		Response = response;
		RawBody = rawBody;
	}
}