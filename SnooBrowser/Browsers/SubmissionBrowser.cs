using System;
using System.Collections.Generic;
using System.Linq;
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

	public async Task<Maybe<(GetSubmissionResponse Submission, IReadOnlyList<Comment> Comments)>> GetSubmission(LinkThing thing)
	{
		var result = (await _snooBrowserHttpClient.TryGet<IReadOnlyList<Listing<JObject>>>(
			UrlHelper.BuildOAuthUrl($"/comments/{thing.ShortId}/")))!;

		if (result is ErrorResponseType)
			return Maybe<(GetSubmissionResponse, IReadOnlyList<Comment>)>.Empty();

		if (result is not SuccessResponseType<IReadOnlyList<Listing<JObject>>> rawData)
			throw new NotImplementedException($"Unknown result type: {result.GetType().FullName}");

		var submission = rawData.Value[0].Data.Children[0];
		var deserializedSubmission = submission.ToObject<ListingKind<GetSubmissionResponse>>()?.Data;

		if (deserializedSubmission?.Url is null)
			throw new JsonSerializationException("Unable to deserialize response");

		var comments = rawData.Value[1].Data.Children
			.Select(x =>
				x.ToObject<ListingKind<Comment>>()?.Data ??
				throw new JsonSerializationException("Unable to deserialize comment"))
			.ToArray();

		return (deserializedSubmission, comments);
	}
}