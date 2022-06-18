using System;
using Newtonsoft.Json;
using SnooBrowser.Things;

namespace SnooBrowser.Models.User;

public record GetAboutUserResponseData
{
	[JsonProperty("is_employee")] public bool IsEmployee { get; init; }

	[JsonProperty("is_friend")] public bool IsFriend { get; init; }

	// FIXME: lots of missing props
	/// <summary>
	/// The user's ID, minus the prefix.
	/// This can be used to create a <see cref="Things.Fullname" />.
	/// </summary>
	/// <example>1d9201j</example>
	public string Id { get; init; } = null!;

	[JsonProperty("verified")] public bool IsVerified { get; init; }
	[JsonProperty("is_gold")] public bool HasGoldStatus { get; init; }
	[JsonProperty("is_mod")] public bool IsMod { get; init; }

	/// <summary>
	/// The user's public name (without /u/ prefixed).
	/// </summary>
	/// <example>AutoModerator</example>
	[JsonProperty("name")]
	public string Username { get; init; } = null!;

	[JsonProperty("created_utc")] public long CreatedAtUtc { get; init; }

	[Obsolete("Obsolete")]
	public Fullname Fullname =>
		Fullname.NewAccount(Id);
}

public record GetAboutUserResponse
{
	public string Kind { get; init; } = null!;
	public GetAboutUserResponseData Data { get; init; } = null!;
}