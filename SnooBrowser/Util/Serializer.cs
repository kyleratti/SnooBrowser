using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SnooBrowser.Util;

public static class Serializer
{
	private static JsonSerializerSettings CreateSettings(NamingStrategy strat) =>
		new()
		{
			DateTimeZoneHandling = DateTimeZoneHandling.Utc,
			ContractResolver = new DefaultContractResolver
			{
				NamingStrategy = strat
			}
		};

	public static readonly JsonSerializerSettings SnakeCaseJsonSerializerSettings =
		CreateSettings(new SnakeCaseNamingStrategy());

	public static readonly JsonSerializerSettings CamelCaseJsonSerializerSettings =
		CreateSettings(new CamelCaseNamingStrategy());
}