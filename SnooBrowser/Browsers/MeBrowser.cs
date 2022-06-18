using System;
using System.Threading.Tasks;
using SnooBrowser.Models.Me;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class MeBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public MeBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<GetMeResponse> GetMe() =>
		(await _snooBrowserHttpClient
			.Get<GetMeResponse>(new Uri(SnooBrowserHttpClient.BaseRedditApiUrl, "api/v1/me")))!;
}