using SnooBrowser.Util;

namespace SnooBrowser.Tests;

public class EnvironmentAuthParameterProvider : IAuthParameterProvider
{
	private const string AppIdEnvironmentVariable = "SnooBrowser__Test__RedditAppId";
	private const string AppSecretEnvironmentVariable = "SnooBrowser__Test__RedditAppSecret";
	private const string RefreshTokenEnvironmentVariable = "SnooBrowser__Test__RedditRefreshToken";

	/// <inheritdoc />
	public string AppId => GetOrThrow(AppIdEnvironmentVariable);

	/// <inheritdoc />
	public string AppSecret => GetOrThrow(AppSecretEnvironmentVariable);

	/// <inheritdoc />
	public string RefreshToken => GetOrThrow(RefreshTokenEnvironmentVariable);

	private static string GetOrThrow(string varName) =>
		Environment.GetEnvironmentVariable(varName) ??
		throw new ArgumentNullException(varName, "Environment variable not set");
}