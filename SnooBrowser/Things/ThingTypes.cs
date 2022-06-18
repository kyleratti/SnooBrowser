namespace SnooBrowser.Things;

public abstract class ThingType
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

public class CommentThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t1";

	/// <inheritdoc />
	public override string ShortId { get; }

	public CommentThing(string shortId)
	{
		ShortId = shortId;
	}
}

public class AccountThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t2";

	/// <inheritdoc />
	public override string ShortId { get; }

	public AccountThing(string shortId)
	{
		ShortId = shortId;
	}
}

public class LinkThing : ThingType
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

public class MessageThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t4";

	/// <inheritdoc />
	public override string ShortId { get; }

	public MessageThing(string shortId)
	{
		ShortId = shortId;
	}
}

public class SubredditThing : ThingType
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

public class AwardThing : ThingType
{
	/// <inheritdoc />
	public override string Prefix => "t6";

	/// <inheritdoc />
	public override string ShortId { get; }

	public AwardThing(string shortId)
	{
		ShortId = shortId;
	}
}