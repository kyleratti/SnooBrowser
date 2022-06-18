using Newtonsoft.Json;

namespace SnooBrowser.Models;

public record RedditUiResponse
{
	[JsonProperty("jquery")] public object[] Jquery { get; init; } = null!;
	[JsonProperty("success")] public bool IsSuccess { get; init; }
}