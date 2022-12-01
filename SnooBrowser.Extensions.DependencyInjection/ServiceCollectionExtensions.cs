using Microsoft.Extensions.DependencyInjection;
using SnooBrowser.Browsers;
using SnooBrowser.Util;

namespace SnooBrowser.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSnooBrowserClient<TAuthParameterProvider, TAccessTokenProvider>(this IServiceCollection serviceCollection)
		where TAuthParameterProvider : class, IAuthParameterProvider
		where TAccessTokenProvider : class, IAccessTokenProvider
	{
		serviceCollection.AddHttpClient<SnooBrowserHttpClient>();
		serviceCollection.AddScoped<IAccessTokenProvider, TAccessTokenProvider>();
		serviceCollection.AddScoped<IAuthParameterProvider, TAuthParameterProvider>();

		serviceCollection.AddScoped<CommentBrowser>();
		serviceCollection.AddScoped<MeBrowser>();
		serviceCollection.AddScoped<NewModmailBrowser>();
		serviceCollection.AddScoped<UserBrowser>();
		serviceCollection.AddScoped<SubredditBrowser>();
		serviceCollection.AddScoped<SubmissionBrowser>();
		serviceCollection.AddScoped<SubredditModerationBrowser>();

		return serviceCollection;
	}
}