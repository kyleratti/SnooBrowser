using System;
using System.Net.Http;

namespace SnooBrowser.Util;

public abstract record HttpResponseType
{
	public TOutput Merge<TSuccess, TOutput>(Func<SuccessResponseType<TSuccess>, TOutput> onSuccess, Func<ErrorResponseType, TOutput> onError) =>
		this switch
		{
			SuccessResponseType<TSuccess> x => onSuccess(x),
			ErrorResponseType x => onError(x),
			_ => throw new ArgumentOutOfRangeException(nameof(HttpResponseType), GetType().FullName, $"Unhandled {nameof(HttpResponseType)}")
		};

	public void WhenError(Action<ErrorResponseType> func)
	{
		if (this is ErrorResponseType x)
			func(x);
	}

	public void When<T>(Action<SuccessResponseType<T>> success, Action<ErrorResponseType> error)
	{
		if (this is SuccessResponseType<T> s)
			success(s);
		else if (this is ErrorResponseType e)
			error(e);

		throw new ArgumentOutOfRangeException(nameof(HttpResponseType), GetType().FullName, $"Unhandled {nameof(HttpResponseType)}");
	}
}

public record SuccessResponseType<T>(HttpResponseMessage Response, T Value) : HttpResponseType;

public record ErrorResponseType(HttpResponseMessage Response, string RawBody) : HttpResponseType;