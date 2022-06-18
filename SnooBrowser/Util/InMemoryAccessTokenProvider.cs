using System;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Models;

namespace SnooBrowser.Util;

public class InMemoryAccessTokenProvider : IAccessTokenProvider
{
	/// <inheritdoc />
	public Maybe<AccessToken> AccessToken { get; private set; } = Maybe<AccessToken>.Empty();

	/// <inheritdoc />
	public Func<AccessToken, Task> OnAccessTokenChanged => newAccessToken =>
	{
		AccessToken = newAccessToken;
		return Task.CompletedTask;
	};
}