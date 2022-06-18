using System.Threading.Tasks;
using SnooBrowser.Models.Subreddit;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class SubredditBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public SubredditBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<GetAboutSubredditResponse> GetAboutSubreddit(string subreddit) =>
		(await _snooBrowserHttpClient.Get<GetAboutSubredditResponse>(UrlHelper.Build($"r/{subreddit}/about.json")))!;
}