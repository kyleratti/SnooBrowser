using System;
using System.Collections.Generic;
using System.Web;
using FruityFoundation.Base.Structures;

namespace SnooBrowser.Util;

public static class UrlHelper
{
	public static Uri Build(string relativeUrl, Maybe<Dictionary<string, string>> queryParams = default)
	{
		var queryString = HttpUtility.ParseQueryString(string.Empty);
		if (queryParams.Try(out var qParams))
		{
			foreach (var (key, value) in qParams)
				queryString.Add(key, value);
		}

		return new Uri(SnooBrowserHttpClient.BaseRedditApiUrl, $"{relativeUrl}{queryParams}");
	}
}