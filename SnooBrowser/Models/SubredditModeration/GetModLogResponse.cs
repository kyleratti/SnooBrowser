using System;
using Newtonsoft.Json;

namespace SnooBrowser.Models.SubredditModeration;

public enum ModLogActionType
{
	RemoveComment = 1,
	Distinguish = 2,
	Sticky = 3,
	IgnoreReports = 4,
	Lock = 5,
	MuteUser = 6,
}

public class ModLogAction
{
	public static ModLogActionType CreateFromString(string str)
	{
		return str switch
		{
			"removecomment" => ModLogActionType.RemoveComment,
			"distinguish" => ModLogActionType.Distinguish,
			"sticky" => ModLogActionType.Sticky,
			"ignorereports" => ModLogActionType.IgnoreReports,
			"lock" => ModLogActionType.Lock,
			"muteuser" => ModLogActionType.MuteUser,
			_ => throw new ArgumentOutOfRangeException(nameof(str), str)
		};
	}
}

public record ModLogEntryData
{
	public string Description { get; init; } = null!;
	public string TargetBody { get; init; } = null!;
	[JsonProperty("mod_id36")] public string ModId36 { get; init; } = null!;
	[JsonProperty("created_utc")] public long CreatedAt { get; init; }
	public string Subreddit { get; init; } = null!;
	public string TargetTitle { get; init; } = null!;
	public string TargetPermalink { get; init; } = null!;
	public string SubredditNamePrefixed { get; init; } = null!;
	public string Details { get; init; } = null!;
	[JsonProperty("action")] public string ActionAsString { get; init; } = null!;
	public ModLogActionType Action => ModLogAction.CreateFromString(ActionAsString);
	public string TargetAuthor { get; init; } = null!;
	public string TargetAuthorFullname { get; init; } = null!; // FIXME:
	[JsonProperty("sr_id36")] public string SubredditFullname { get; init; } = null!; // FIXME:
	public string Id { get; init; } = null!; // FIXME:
	public string ModUsername { get; init; } = null!; // FIXME:
}

public record ModLogEntry
{
	public string Kind { get; init; } = null!; // FIXME:

	public ModLogEntryData Data { get; init; } = null!; // FIXME: it would be nice to have these on the root ModLogEntry object
}

public record ListingData
{
	public string After { get; init; } = null!;
	public string Dist { get; init; } = null!; // FIXME:
	[JsonProperty("modhash")] public string ModHash = null!;
	[JsonProperty("geofilter")] public string GeoFilter = null!;
	[JsonProperty("children")] public ModLogEntry[] Entries = null!;
}

public record GetModLogResponse
{
	public string Kind { get; init; } = null!; // FIXME:
	public ListingData Data { get; init; } = null!;
}