using SnooBrowser.Browsers;

namespace SnooBrowser.Tests;

public class MeBrowserTests : BrowserTestsBase
{
	private MeBrowser _meBrowser = null!;

	[OneTimeSetUp]
	public void Setup()
	{
		_meBrowser = GetService<MeBrowser>();
	}

	[Test]
	public async Task TestGetMe()
	{
		var me = await _meBrowser.GetMe();
		
		Assert.That(me.Username, Is.EqualTo("snoobrowser-testing"));
		Assert.That(me.Id36, Is.EqualTo("qqldyszc"));
	}
}