using System.Threading.Tasks;
using SnooBrowser.Models.User;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class UserBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public UserBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<GetAboutUserResponse> GetAboutUser(string username) =>
		(await _snooBrowserHttpClient.Get<GetAboutUserResponse>(UrlHelper.Build($"user/{username}/about")))!;
}