using Newtonsoft.Json;

namespace SnooBrowser.Models;

internal record ListingKind<T>([property:JsonProperty("data")] T Data);

internal record ListingData<T>
{
	public T[] Children { get; }

	[JsonConstructor]
	internal ListingData(T[] children)
	{
		Children = children;
	}
}

internal record Listing<T>
{
	
	public ListingData<T> Data { get; }

	[JsonConstructor]
	internal Listing(
		ListingData<T> data
	)
	{
		Data = data;
	}
}