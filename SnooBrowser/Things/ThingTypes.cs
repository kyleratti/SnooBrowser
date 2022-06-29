using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SnooBrowser.Things;

public abstract record ThingType
{
	/// <summary>
	/// The prefix of the thing <b>without</b> the trailing <c>_</c> character.
	/// </summary>
	/// <example>"t1"</example>
	public abstract string Prefix { get; }
	/// <summary>
	/// The Short ID of the thing with no prefix.
	/// </summary>
	/// <example>"ad891"</example>
	public abstract string ShortId { get; }
	/// <summary>
	/// The Full ID of the thing, which is a combination of the prefix and short ID.
	/// </summary>
	/// <example>"t1_ad891"</example>
	public string FullId => $"{Prefix}_{ShortId}";
}

[JsonConverter(typeof(ThingConverter))]
public record CommentThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t1";

	/// <inheritdoc />
	public override string ShortId { get; }

	private CommentThing(string shortId)
	{
		ShortId = shortId;
	}

	public static CommentThing CreateFromShortId(string shortId) =>
		new(shortId);
	public static CommentThing CreateFromFullId(string fullId) =>
		new(fullId[(fullId.IndexOf('_') + 1)..]);
}

[JsonConverter(typeof(ThingConverter))]
public record AccountThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t2";

	/// <inheritdoc />
	public override string ShortId { get; }

	private AccountThing(string shortId)
	{
		ShortId = shortId;
	}

	public static AccountThing CreateFromShortId(string shortId) =>
		new(shortId);
	public static AccountThing CreateFromFullId(string fullId) =>
		new(fullId[(fullId.IndexOf('_') + 1)..]);
}

[JsonConverter(typeof(ThingConverter))]
public record LinkThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t3";

	/// <inheritdoc />
	public override string ShortId { get; }

	private LinkThing(string shortId)
	{
		ShortId = shortId;
	}

	public static LinkThing CreateFromShortId(string shortId) =>
		new(shortId);
	public static LinkThing CreateFromFullId(string fullId) =>
		new(fullId[(fullId.IndexOf('_') + 1)..]);
}

[JsonConverter(typeof(ThingConverter))]
public record MessageThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t4";

	/// <inheritdoc />
	public override string ShortId { get; }

	private MessageThing(string shortId)
	{
		ShortId = shortId;
	}

	public static MessageThing CreateFromShortId(string shortId) =>
		new(shortId);
	public static MessageThing CreateFromFullId(string fullId) =>
		new(fullId[(fullId.IndexOf('_') + 1)..]);
}

[JsonConverter(typeof(ThingConverter))]
public record SubredditThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t5";

	/// <inheritdoc />
	public override string ShortId { get; }

	private SubredditThing(string shortId)
	{
		ShortId = shortId;
	}

	public static SubredditThing CreateFromShortId(string shortId) =>
		new(shortId);
	public static SubredditThing CreateFromFullId(string fullId) =>
		new(fullId[(fullId.IndexOf('_') + 1)..]);
}

[JsonConverter(typeof(ThingConverter))]
public record AwardThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t6";

	/// <inheritdoc />
	public override string ShortId { get; }

	private AwardThing(string shortId)
	{
		ShortId = shortId;
	}

	public static AwardThing CreateFromShortId(string shortId) =>
		new(shortId);
	public static AwardThing CreateFromFullId(string fullId) =>
		new(fullId[(fullId.IndexOf('_') + 1)..]);
}

public class ThingConverter : JsonConverter
{
	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		if (value is ThingType tt)
			writer.WriteValue(tt.FullId);
		else
			throw new NotImplementedException();
	}

	/*
	 * Matches: t1_asldkj129
	 * Matches: t5_392401k
	 * Matches: t6_1wke
	 * No Match: t7_1023
	 * No Match: t71230
	 * No Match: 1293
	 * No Match: t6_
	 */
	private readonly Regex _validFullname =
		new(@"^t(?<typeId>[1-6])_(?<shortId>[a-zA-Z0-9]{1,})$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

	/// <inheritdoc />
	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
		JsonSerializer serializer)
	{
		if (reader.Value is null)
			return null;

		if (reader.Value is not string value)
			throw new ArgumentOutOfRangeException(nameof(reader.Value), reader.Value, "Received unexpected data type");

		var result = _validFullname.Match(value);

		if (!result.Success)
			throw new JsonSerializationException($"Unable to detect a valid {nameof(LinkThing)} from value: {value}");

		var typeId = Convert.ToInt32(result.Groups["typeId"].Value);
		var shortId = result.Groups["shortId"].Value;

		return typeId switch
		{
			1 => CommentThing.CreateFromShortId(shortId),
			2 => AccountThing.CreateFromShortId(shortId),
			3 => LinkThing.CreateFromShortId(shortId),
			4 => MessageThing.CreateFromShortId(shortId),
			5 => SubredditThing.CreateFromShortId(shortId),
			6 => AwardThing.CreateFromShortId(shortId),
			_ => throw new NotImplementedException($"{nameof(ThingConverter)} not implemented for: {existingValue}")
		};
	}

	/// <inheritdoc />
	public override bool CanConvert(Type objectType) =>
		objectType.IsAssignableFrom(typeof(CommentThing))
		|| objectType.IsAssignableFrom(typeof(AccountThing))
		|| objectType.IsAssignableFrom(typeof(LinkThing))
		|| objectType.IsAssignableFrom(typeof(MessageThing))
		|| objectType.IsAssignableFrom(typeof(SubredditThing))
		|| objectType.IsAssignableFrom(typeof(AwardThing));
}