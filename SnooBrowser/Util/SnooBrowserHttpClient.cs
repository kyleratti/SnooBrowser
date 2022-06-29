using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnooBrowser.Models;
using SnooBrowser.Models.Auth;

namespace SnooBrowser.Util;

public class SnooBrowserHttpClient
{
	public static readonly Uri BaseRedditOAuthApiUrl = new("https://oauth.reddit.com/");
	public static readonly Uri BaseRedditLegacyApiUrl = new("https://reddit.com/");
	private readonly HttpClient _httpClient;
	private readonly IAuthParameterProvider _authParameterProvider;
	private readonly IAccessTokenProvider _accessTokenProvider;

	private readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);

	public SnooBrowserHttpClient(HttpClient httpClient, IAuthParameterProvider authParameterProvider, IAccessTokenProvider accessTokenProvider)
	{
		_httpClient = httpClient;
		_authParameterProvider = authParameterProvider;
		_accessTokenProvider = accessTokenProvider;
	}

	public async Task<T?> SendAndParseAsJson<T>(HttpMethod httpMethod, Uri url, MessageBodyType bodyType, object? body = null) =>
		await SendAndParseAsJsonImpl<T>(triesRemaining: 2,
			httpMethod,
			url,
			new BearerTokenAuthenticationType(_accessTokenProvider.AccessToken.Map(x => x.Token).OrValue("")),
			bodyType,
			body);
	
	public async Task<HttpResponseType> SendAndTryParseAsJson<T>(HttpMethod httpMethod, Uri url, MessageBodyType bodyType, object? body = null) =>
		await SendAndTryParseAsJson<T>(triesRemaining: 2,
			httpMethod,
			url,
			new BearerTokenAuthenticationType(_accessTokenProvider.AccessToken.Map(x => x.Token).OrValue("")),
			bodyType,
			body);

	public async Task<T?> Post<T>(Uri url, MessageBodyType? bodyType = MessageBodyType.None, object? body = null) =>
		await SendAndParseAsJson<T>(HttpMethod.Post, url, bodyType ?? MessageBodyType.None, body);

	public async Task Post(Uri url, MessageBodyType? bodyType = MessageBodyType.None, object? body = null) =>
		await SendAndParseAsJson<object>(HttpMethod.Post, url, bodyType ?? MessageBodyType.None, body);

	public async Task<T?> Get<T>(Uri url) =>
		await SendAndParseAsJson<T>(HttpMethod.Get, url, MessageBodyType.None);

	public async Task<HttpResponseType> TryGet<T>(Uri url) =>
		await SendAndTryParseAsJson<T>(HttpMethod.Get, url, MessageBodyType.None);

	private async Task<T?> SendAndParseAsJsonImpl<T>(int triesRemaining, HttpMethod httpMethod, Uri url,
		HttpAuthenticationType authType, MessageBodyType bodyType, object? body = null)
	{
		var resp = await SendImpl(triesRemaining, httpMethod, url, authType, bodyType, body);
		var content = await AssertIsSuccessfulResponse(resp);

		return JsonConvert.DeserializeObject<T>(content);
	}

	private async Task<HttpResponseType> SendAndTryParseAsJson<T>(int triesRemaining, HttpMethod httpMethod, Uri url,
		HttpAuthenticationType authType, MessageBodyType bodyType, object? body = null)
	{
		var resp = await SendImpl(triesRemaining, httpMethod, url, authType, bodyType, body);
		var content = await resp.Content.ReadAsStringAsync();

		if (!resp.IsSuccessStatusCode)
			return new ErrorResponseType(resp, content);

		return new SuccessResponseType<T?>(resp, JsonConvert.DeserializeObject<T>(content));
	}

	private async Task<HttpResponseMessage> SendImpl(int triesRemaining, HttpMethod httpMethod, Uri url,
		HttpAuthenticationType authType, MessageBodyType bodyType, object? body = null)
	{
		async Task<HttpResponseMessage> RefreshAccessTokenAndRetry(int remaining)
		{
			var newAccessToken = await RefreshAccessToken();
			var newAuthType = new BearerTokenAuthenticationType(newAccessToken.Token);
			return await SendImpl(remaining, httpMethod, url, newAuthType, bodyType, body);
		}

		while (triesRemaining > 0)
		{
			triesRemaining--;

			// We know to run immediately if the access token is blank
			if (authType is BearerTokenAuthenticationType bearerTokenAuthType && string.IsNullOrEmpty(bearerTokenAuthType.AccessToken))
				return await RefreshAccessTokenAndRetry(triesRemaining);

			using var req = new HttpRequestMessage
			{
				Method = httpMethod,
				RequestUri = url,
				Headers =
				{
					Authorization = authType switch
					{
						BearerTokenAuthenticationType x => new AuthenticationHeaderValue("Bearer", x.AccessToken),
						BasicAuthenticationType x => new AuthenticationHeaderValue("Basic", x.EncodedValue),
						_ => throw new NotImplementedException($"Unhandled {nameof(HttpAuthenticationType)}: {authType.GetType().FullName}")
					}
				},
				Content = MessageBodyTypeToHttpContent(bodyType, body)
			};
			req.Headers.TryAddWithoutValidation("User-Agent", "SnooBrowser by /u/rylekatti (v1.0.0)");
			var resp = await _httpClient.SendAsync(req);

			if (await IsErrorFromExpiredAccessToken(resp))
			{
				var newAccessToken = await RefreshAccessToken();
				var newAuthType = new BearerTokenAuthenticationType(newAccessToken.Token);
				return await SendImpl(triesRemaining, httpMethod, url, newAuthType, bodyType, body);
			}

			return resp;
		}

		throw new Exception("Retry count exceeded");
	}

	private static Task<bool> IsErrorFromExpiredAccessToken(HttpResponseMessage resp)
	{
		if (resp.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
			return Task.FromResult(true);

		return Task.FromResult(false);
	}

	private async Task<AccessToken> RefreshAccessToken()
	{
		var originalToken = _accessTokenProvider.AccessToken;

		await _semaphore.WaitAsync();

		if (_accessTokenProvider.AccessToken.HasValue && _accessTokenProvider.AccessToken.Equals(originalToken))
			return _accessTokenProvider.AccessToken.Value;

		try
		{
			var reqStartedAt = DateTime.Now;
			var resp = await SendAndParseAsJsonImpl<GetAccessTokenResponse>(
				triesRemaining: 1,
				HttpMethod.Post,
				new Uri("https://www.reddit.com/api/v1/access_token"),
				new BasicAuthenticationType(_authParameterProvider.AppId, _authParameterProvider.AppSecret),
				MessageBodyType.FormUrlEncoded, new
				{
					grant_type = "refresh_token",
					refresh_token = _authParameterProvider.RefreshToken
				});

			var newAccessToken = new AccessToken(resp!.AccessToken, reqStartedAt.AddSeconds(resp.ExpiresIn - (60 * 2)));
			await _accessTokenProvider.OnAccessTokenChanged.Invoke(newAccessToken);

			return newAccessToken;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	private static HttpContent? MessageBodyTypeToHttpContent(MessageBodyType bodyType, object? body)
	{
		if (body is null)
			return null;
		
		return bodyType switch
		{
			MessageBodyType.None => null,
			MessageBodyType.Json => new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
			MessageBodyType.FormUrlEncoded => new FormUrlEncodedContent(JObject.FromObject(body).ToObject<Dictionary<string, string>>()!),
			_ => throw new NotImplementedException($"{nameof(MessageBodyType)} not implemented: {bodyType:D}")
		};
	}

	private static async Task<string> AssertIsSuccessfulResponse(HttpResponseMessage resp)
	{
		var content = await resp.Content.ReadAsStringAsync();

		// FIXME: this sucks
		if (!resp.IsSuccessStatusCode)
			throw new ApplicationException($"Received Non-Success Status Code: {resp.StatusCode:G} ({resp.StatusCode:D}), body: {content}");

		return content;
	}
}

public enum MessageBodyType : byte
{
	None = 1,
	Json = 2,
	FormUrlEncoded = 3
}