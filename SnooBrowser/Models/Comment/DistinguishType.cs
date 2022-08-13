using System;
using Newtonsoft.Json;

namespace SnooBrowser.Models.Comment;

[JsonConverter(typeof(DistinguishTypeConverter))]
public record DistinguishType
{
	public string ApiValue => RawType switch
	{
		TypeId.None => "no",
		TypeId.Moderator => "yes",
		TypeId.Admin => "admin",
		TypeId.Special => "special",
		_ => throw new ArgumentOutOfRangeException(nameof(DistinguishType), GetType().FullName, $"Unhandled {nameof(DistinguishType)}")
	};

	public static readonly DistinguishType None = new(TypeId.None);
	public static readonly DistinguishType Moderator = new(TypeId.Moderator);
	public static readonly DistinguishType Admin = new(TypeId.Admin);
	public static readonly DistinguishType Special = new(TypeId.Special);

	public static DistinguishType ParseFromString(string? input) => input?.ToLower() switch
	{
		null or "no" => DistinguishType.None,
		"moderator" or "yes" => DistinguishType.Moderator,
		"admin" => DistinguishType.Admin,
		"special" => DistinguishType.Special,
		_ => throw new ArgumentOutOfRangeException(nameof(input), input, $"Unhandled {nameof(DistinguishType)}")
	};

	private TypeId RawType { get; }

	private DistinguishType(TypeId rawType)
	{
		RawType = rawType;
	}

	private enum TypeId
	{
		None = 1,
		Moderator = 2,
		Admin = 3,
		Special = 4
	}
}

public class DistinguishTypeConverter : JsonConverter<DistinguishType>
{
	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, DistinguishType? value, JsonSerializer serializer)
	{
		ArgumentNullException.ThrowIfNull(value);

		writer.WriteValue(value.ApiValue);
	}

	/// <inheritdoc />
	public override DistinguishType? ReadJson(JsonReader reader, Type objectType, DistinguishType? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.Value is null)
			return DistinguishType.None;
		else if (reader.Value is string s)
			return DistinguishType.ParseFromString(s);
		
		throw new ArgumentOutOfRangeException(nameof(reader.Value), reader.Value?.GetType().FullName, "Unhandled value type");
	}
}