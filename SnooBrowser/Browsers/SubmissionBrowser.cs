using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnooBrowser.Models;
using SnooBrowser.Models.Comment;
using SnooBrowser.Models.Submission;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class SubmissionBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public SubmissionBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<Maybe<Submission>> GetSubmission(LinkThing thing)
	{
		var result = (await _snooBrowserHttpClient.TryGet<IReadOnlyList<Listing<JObject>>>(
			UrlHelper.BuildOAuthUrl($"/comments/{thing.ShortId}/")));

		if (result is ErrorResponseType e)
		{
			if (e.Response.StatusCode is HttpStatusCode.NotFound && 
				JsonConvert.DeserializeObject<SubmissionNotFound>(e.RawBody) is { StatusCode: HttpStatusCode.NotFound })
			{
				return Maybe<Submission>.Empty();
			}

			throw new ApplicationException(e.RawBody);
		}

		if (result is not SuccessResponseType<IReadOnlyList<Listing<JObject>>> rawData)
			throw new NotImplementedException($"Unhandled result type: {result.GetType().FullName}");

		var submission = rawData.Value[0].Data.Children[0];
		var deserializedSubmission = submission.ToObject<ListingKind<GetSubmissionResponse>>()?.Data;

		if (deserializedSubmission?.Url is null)
			throw new JsonSerializationException("Unable to deserialize response");

		var comments = rawData.Value[1].Data.Children
			.Select(x =>
				x.ToObject<ListingKind<Comment>>()?.Data ??
				throw new JsonSerializationException("Unable to deserialize comment"))
			.ToArray();

		return new Submission(
			deserializedSubmission.Subreddit,
			SelfText: Maybe<string>.Create(deserializedSubmission.SelfText!, hasValue: deserializedSubmission.IsSelfPost),
			deserializedSubmission.Title,
			deserializedSubmission.SubmissionFullname,
			deserializedSubmission.IsSelfPost,
			deserializedSubmission.IsArchived,
			deserializedSubmission.IsLocked,
			deserializedSubmission.SubredditFullname,
			deserializedSubmission.RedditPostId,
			AuthorName: Maybe<string>.Create(deserializedSubmission.AuthorName!, hasValue: !string.IsNullOrEmpty(deserializedSubmission.AuthorName)),
			deserializedSubmission.CommentCount,
			deserializedSubmission.Permalink,
			deserializedSubmission.Url,
			comments);
	}

	/// <summary>
	/// The API response for getting a submission.
	/// </summary>
	/// <param name="Subreddit">The name of the subreddit (e.g., "aww")</param>
	/// <param name="SelfText">The text of the self post. If there is no text, this is Empty.</param>
	/// <param name="Title">The title of the post.</param>
	/// <param name="SubmissionFullname">The fullname of the submission, including thing prefix.</param>
	/// <param name="IsSelfPost"></param>
	/// <param name="IsArchived"></param>
	/// <param name="IsLocked"></param>
	/// <param name="SubredditFullname">The fullname of the subreddit, including thing prefix.</param>
	/// <param name="RedditPostId">The reddit post ID (e.g., "vf79a9")</param>
	/// <param name="AuthorName">The author name without any prefixes. If the author has been deleted, this is Empty.</param>
	/// <param name="CommentCount">The total number of comments.</param>
	/// <param name="Permalink">The permalink relative to reddit's domain (e.g., "/r/AskProgramming/comments/vf79a9/are_rich_client_apps_still_a_thing/")</param>
	/// <param name="Url">The full URL, including scheme and hostname, to the submission (e.g., "https://www.reddit.com/r/AskProgramming/comments/vf79a9/are_rich_client_apps_still_a_thing/")</param>
	private record GetSubmissionResponse(
		[JsonProperty("subreddit")] string Subreddit,
		[JsonProperty("selfText")] string? SelfText,
		[JsonProperty("title")] string Title,
		[JsonProperty("name")] LinkThing SubmissionFullname,
		[JsonProperty("is_self")] bool IsSelfPost,
		[JsonProperty("archived")] bool IsArchived,
		[JsonProperty("locked")] bool IsLocked,
		[JsonProperty("subreddit_id")] SubredditThing SubredditFullname,
		[JsonProperty("id")] string RedditPostId,
		[JsonProperty("author")] string? AuthorName,
		[JsonProperty("num_comments")] int CommentCount,
		[JsonProperty("permalink")] string Permalink,
		[JsonProperty("url")] string Url);
}