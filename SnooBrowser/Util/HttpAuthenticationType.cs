using System;
using System.Text;

namespace SnooBrowser.Util;

public record HttpAuthenticationType;

public record BearerTokenAuthenticationType(string AccessToken) : HttpAuthenticationType;

public record BasicAuthenticationType(string Username, string Password) : HttpAuthenticationType
{
	public string EncodedValue => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
}