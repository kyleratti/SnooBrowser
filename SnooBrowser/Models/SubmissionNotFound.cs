using System.Net;
using Newtonsoft.Json;

namespace SnooBrowser.Models;

public record SubmissionNotFound(
	[JsonProperty("message")] string Message,
	[JsonProperty("error")] HttpStatusCode StatusCode);