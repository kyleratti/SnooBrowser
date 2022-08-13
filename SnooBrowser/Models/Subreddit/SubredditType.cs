using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SnooBrowser.Models.Subreddit;

/// <summary>
/// The type of subreddit this is.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SubredditType
{
	[EnumMember(Value = "public")] Public = 1,
	[EnumMember(Value = "private")] Private = 2,
	[EnumMember(Value = "restricted")] Restricted = 3
}