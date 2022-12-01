using System;
using System.Reflection;

namespace SnooBrowser.Util;

public static class LibraryMetadataHelper
{
	public const string AuthorRedditUsername = @"/u/rylekatti";
	public static Version Version => Assembly.GetExecutingAssembly().GetName().Version ?? throw new Exception("Unable to find version of executing assembly");
}