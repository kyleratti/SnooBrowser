using System.Threading.Tasks;
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

	public async Task<GetSubmissionResponse> GetSubmission(LinkThing thing) =>
		(await _snooBrowserHttpClient.Get<GetSubmissionResponse>(
			UrlHelper.Build($"{thing.ShortId}.json")))!;
}