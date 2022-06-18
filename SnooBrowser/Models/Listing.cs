using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SnooBrowser.Models;

internal record ListingData
{
	public JObject[] Children { get; }

	[JsonConstructor]
	internal ListingData(JObject[] children)
	{
		Children = children;
	}
}

internal record Listing
{
	
	public ListingData Data { get; }

	[JsonConstructor]
	internal Listing(
		ListingData data
	)
	{
		Data = data;
	}
}