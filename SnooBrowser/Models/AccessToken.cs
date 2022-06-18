using System;

namespace SnooBrowser.Models;

public record AccessToken(string Token,	DateTime ExpiresAt)
{
	public bool IsExpired => ExpiresAt < DateTime.Now;
}