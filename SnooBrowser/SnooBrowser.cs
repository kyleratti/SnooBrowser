using SnooBrowser.Util;

namespace SnooBrowser;

public sealed class SnooBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public SnooBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}
}
