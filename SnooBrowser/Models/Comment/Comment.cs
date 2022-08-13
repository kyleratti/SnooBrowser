using System;
using Newtonsoft.Json;
using SnooBrowser.Models.Subreddit;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Models.Comment;

/// <summary>
/// A comment
/// </summary>
/// <param name="RawCommentId">The comment ID with no t1_ prefix</param>
/// <param name="Score"></param>
/// <param name="AuthorName"></param>
/// <param name="RawAuthorId"></param>
/// <param name="SubredditName">The subreddit's name with no /r/ prefix.</param>
/// <param name="RawSubredditId"></param>
/// <param name="RawParentId"></param>
/// <param name="RawCreatedAtUtc"></param>
/// <param name="BodyAsMarkdown"></param>
/// <param name="BodyAsEncodedHtml"></param>
/// <param name="IsStickied"></param>
/// <param name="Permalink"></param>
/// <param name="SubredditType"></param>
/// <param name="DistinguishType"></param>
/// <param name="IsArchived"></param>
/// <param name="IsEdited"></param>
public record Comment(
	[JsonProperty("name")] string RawCommentId,
	[JsonProperty("score")] int Score,
	[JsonProperty("author")] string AuthorName,
	[JsonProperty("author_fullname")] string RawAuthorId,
	[JsonProperty("subreddit")] string SubredditName,
	[JsonProperty("subreddit_id")] string RawSubredditId,
	[JsonProperty("parent_id")] string RawParentId,
	[JsonProperty("created_utc")] double RawCreatedAtUtc,
	[JsonProperty("body")] string BodyAsMarkdown,
	[JsonProperty("body_html")] string BodyAsEncodedHtml,
	[JsonProperty("stickied")] bool IsStickied,
	[JsonProperty("permalink")] string Permalink,
	[JsonProperty("subreddit_type")] SubredditType SubredditType,
	[JsonProperty("distinguished")] DistinguishType DistinguishType,
	[JsonProperty("archived")] bool IsArchived,
	[JsonProperty("edited")] bool IsEdited
)
{
	public CommentThing CommentId => CommentThing.CreateFromFullId(RawCommentId);
	public AccountThing AuthorId => AccountThing.CreateFromFullId(RawAuthorId);
	public SubredditThing SubredditId => SubredditThing.CreateFromFullId(RawSubredditId);
	public OneOf<LinkThing, CommentThing> ParentId => ThingType.ParseFromStringOrFail(RawParentId).Merge(
		linkThing: OneOf<LinkThing, CommentThing>.CreateOne,
		commentThing: OneOf<LinkThing, CommentThing>.CreateTwo,
		accountThing: _ => throw new ArgumentOutOfRangeException(nameof(RawParentId), RawParentId, "Unsupported type"),
		subredditThing: _ => throw new ArgumentOutOfRangeException(nameof(RawParentId), RawParentId, "Unsupported type"),
		awardThing: _ => throw new ArgumentOutOfRangeException(nameof(RawParentId), RawParentId, "Unsupported type"),
		messageThing: _ => throw new ArgumentOutOfRangeException(nameof(RawParentId), RawParentId, "Unsupported type"));
	public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(RawCreatedAtUtc));
}