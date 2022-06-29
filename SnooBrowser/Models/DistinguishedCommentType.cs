using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SnooBrowser.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum DistinguishedCommentType
{
	None = 0,
	[EnumMember(Value = "moderator")] Moderator = 1
}