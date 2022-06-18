using Newtonsoft.Json;

namespace SnooBrowser.Models.Auth;

public record GetAccessTokenResponse
{
	[JsonProperty("access_token")] public string AccessToken = null!;
	[JsonProperty("token_type")] public string TokenType = null!; // FIXME:

	/// <summary>
	/// The time, in Unix Epoch seconds, at which the <see cref="AccessToken"/> expires.
	/// </summary>
	[JsonProperty("expires_in")] public long ExpiresIn;

	[JsonProperty("scope")] public string Scope = null!;
	[JsonProperty("refresh_token")] public string RefreshToken = null!; // FIXME: should be a Maybe
}