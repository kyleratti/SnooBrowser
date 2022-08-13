using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnooBrowser.Models.Comment;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class CommentBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public CommentBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<Comment> SubmitComment(OneOf<CommentThing, LinkThing> parent, string markdownText)
	{
		var parentThingId = parent.Map(ct => ct.FullId, lt => lt.FullId);

		var resp = await _snooBrowserHttpClient.TryPost<RawSubmitCommentResponse>(UrlHelper.BuildOAuthUrl("api/comment"), MessageBodyType.FormUrlEncoded, new
		{
			api_type = "json",
			return_rtjson = false,
			text = markdownText,
			thing_id = parentThingId
		});

		var data = resp switch
		{
			ErrorResponseType err => throw new Exception(err.RawBody), // FIXME:
			SuccessResponseType<RawSubmitCommentResponse> success => success.Value,
			_ => throw new NotImplementedException($"Response type not implemented: {resp.GetType().FullName}")
		};

		return ParseSingleCommentOrFail(data);
	}

	public async Task<Comment> DistinguishComment(CommentThing comment, DistinguishType distinguishType, bool? isSticky)
	{
		var resp = await _snooBrowserHttpClient.TryPost<RawSubmitCommentResponse>(UrlHelper.BuildOAuthUrl("api/distinguish"),
			MessageBodyType.FormUrlEncoded, new
			{
				api_type = "json",
				how = distinguishType.ApiValue,
				id = comment.FullId,
				sticky = isSticky
			});

		return resp.Merge<RawSubmitCommentResponse, Comment>(onSuccess: x => ParseSingleCommentOrFail(x.Value),
			onError: x => throw new Exception(x.RawBody));
	}

	private static Comment ParseSingleCommentOrFail(RawSubmitCommentResponse input)
	{
		if (input.Json.Errors.Any())
			throw new Exception($"Error submitting comment: {JsonConvert.SerializeObject(input.Json.Errors)}");

		if (!input.Json.Data.TryGetValue("things", out var things))
			throw new Exception($"Expected new comment data in response but no data was found: {JsonConvert.SerializeObject(input)}");

		var childCount = things.Children().Count();
		if (childCount != 1)
			throw new ArgumentOutOfRangeException(nameof(childCount), childCount, "Expected to receive back 1 comment");

		var firstThing = things.First().ToObject<JObject>()!;
		if (!firstThing.TryGetValue("kind", out var kind) && kind!.ToObject<string>() != "t1")
			throw new Exception($"Unable to get kind from thing: {JsonConvert.SerializeObject(firstThing)}");

		return firstThing["data"]!.ToObject<Comment>()!;
	}

	private record SubmitCommentJsonResponse(
		[JsonProperty("errors")] object[] Errors, // Seems to be type (string ErrorType, string Message, string ErrorTypeButLowercase)[]
		[JsonProperty("data")] JObject Data
	);
	private record RawSubmitCommentResponse([JsonProperty("json")] SubmitCommentJsonResponse Json);
}