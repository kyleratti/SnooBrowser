using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SnooBrowser.Things;

namespace SnooBrowser.Models.NewModmail;

public record ObjectId
{
	public string Id { get; init; } = null!;
	public string Key { get; init; } = null!;
}

public record ConversationOwner
{
	public string DisplayName { get; init; } = null!;
	public string Type { get; init; } = null!; // FIXME:
	public string Id { get; init; } = null!; // FIXME:
}

public record Conversation
{
	/// <summary>
	/// Whether this conversation was started by /u/AutoModerator.
	/// </summary>
	[JsonProperty("isAuto")]
	public bool IsStartedByAutoMod { get; init; }

	public Author Participant { get; init; } = null!;
	[JsonProperty("objIds")] public ObjectId[] ObjectIds = null!;
	public bool IsRepliable { get; init; }
	public DateTime? LastUserUpdate { get; init; }
	public bool IsInternal { get; init; }
	public DateTime? LastModUpdate { get; init; }
	public Author[] Authors = null!;
	public DateTime LastUpdated { get; init; }
	public string LegacyFirstMessageId { get; init; } = null!;
	public ModmailState State { get; init; }
	public DateTime? LastUnread { get; init; }
	public ConversationOwner Owner { get; init; } = null!;
	public string Subject { get; init; } = null!;
	public string Id { get; init; } = null!; // FIXME:
	public bool IsHighlighted { get; init; }
	[JsonProperty("numMessages")] public int MessageCount { get; init; }
}

public record ConversationMessage
{
	[JsonProperty("body")] public string BodyAsHtml { get; init; } = null!;
	[JsonProperty("bodyMarkdown")] public string BodyAsMarkdown { get; init; } = null!;
	public Author Author { get; init; } = null!;
	public bool IsInternal { get; init; }
	[JsonProperty("date")] public DateTime SentAt { get; init; }
	public string Id { get; init; } = null!; // FIXME:
}

public record GetConversationsResponse
{
	public Dictionary<string, Conversation> Conversations = null!;
	public Dictionary<string, ConversationMessage> Messages = null!;
}