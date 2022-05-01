using Newtonsoft.Json;
using SnooBrowser.Things;

namespace SnooBrowser.Structures.Subreddit
{
    public record GetAboutSubredditResponseData
    {
        [JsonProperty("display_name")] public string DisplayName { get; init; } = null!;
        public string Title { get; init; } = null!;
        [JsonProperty("display_name_prefixed")] public string DisplayNamePrefixed { get; init; } = null!;
        public string Id { get; init; } = null!;
        [JsonProperty("created_utc")] public long CreatedAtUtc { get; init; }

        public Fullname Fullname =>
            Fullname.NewSubreddit(Id);
    }

    public record GetAboutSubredditResponse
    {
        public string Kind { get; init; } = null!;
        public GetAboutSubredditResponseData Data { get; init; } = null!;
    }
}
