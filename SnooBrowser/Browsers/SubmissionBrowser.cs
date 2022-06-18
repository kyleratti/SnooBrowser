using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Models;
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

	public async Task<Maybe<GetSubmissionResponse>> GetSubmission(LinkThing thing)
	{
		var result = (await _snooBrowserHttpClient.TryGet<IReadOnlyList<Listing>>(
			UrlHelper.Build($"{thing.ShortId}.json")))!;

		if (result is ErrorResponseType)
			return Maybe<GetSubmissionResponse>.Empty();

		if (result is not SuccessResponseType<IReadOnlyList<Listing>> rawData)
			throw new NotImplementedException($"Unknown result type: {result.GetType().FullName}");

		var resp = rawData.Value[0].Data.Children[0];

		return resp.ToObject<GetSubmissionResponse>()!;
	}
}