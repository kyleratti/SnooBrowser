using Microsoft.Extensions.DependencyInjection;
using SnooBrowser.Extensions.DependencyInjection;
using SnooBrowser.Util;

namespace SnooBrowser.Tests;

public class BrowserTestsBase
{
	private IServiceScope _serviceScope = null!;
	private IServiceProvider _serviceProvider = null!;

	[OneTimeSetUp]
	public void BaseSetup()
	{
		var services = new ServiceCollection();
		services.AddSnooBrowserClient<EnvironmentAuthParameterProvider, InMemoryAccessTokenProvider>();

		var provider = services.BuildServiceProvider();
		_serviceProvider = provider;
		_serviceScope = _serviceProvider.CreateScope();
	}

	[OneTimeTearDown]
	public void BaseTearDown()
	{
		_serviceScope.Dispose();
	}

	protected T GetService<T>() where T : notnull =>
		_serviceProvider.GetRequiredService<T>();
}