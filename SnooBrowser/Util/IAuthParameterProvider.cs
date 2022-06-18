namespace SnooBrowser.Util;

public interface IAuthParameterProvider
{
	/// <summary>
	/// The application ID
	/// </summary>
	public string AppId { get; }
	/// <summary>
	/// The application secret
	/// </summary>
	public string AppSecret { get; }
	/// <summary>
	/// The application's current refresh token
	/// </summary>
	public string RefreshToken { get; }
}