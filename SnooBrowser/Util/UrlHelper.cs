using System;
using System.Collections.Generic;
using System.Web;
using FruityFoundation.Base.Structures;

namespace SnooBrowser.Util;

public static class UrlHelper
{
	public static Uri BuildOAuthUrl(string relativeUrl, Maybe<Dictionary<string, string>> queryParams = default) =>
		new(SnooBrowserHttpClient.BaseRedditOAuthApiUrl,
			$"{relativeUrl}{queryParams.Map(BuildQueryString).OrValue(string.Empty)}");

	private static string BuildQueryString(Dictionary<string, string> queryParams)
	{
		var queryString = HttpUtility.ParseQueryString(string.Empty);
		foreach(var (key, value) in queryParams)
			queryString.Add(key, value);

		return queryString.ToString()!;
	}
}