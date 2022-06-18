using System;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Models;

namespace SnooBrowser.Util;

public interface IAccessTokenProvider
{
	public Maybe<AccessToken> AccessToken { get; }
	/// <summary>
	/// Callback fired when the access token has been changed (refreshed)
	/// </summary>
	public Func<AccessToken, Task> OnAccessTokenChanged { get; }
}