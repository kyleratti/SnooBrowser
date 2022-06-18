using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SnooBrowser.Things;

namespace SnooBrowser.Models.NewModmail;

public record MuteStatus
{
	public int MuteCount { get; init; }
	public bool IsMuted { get; init; }
	public DateTime? EndDate { get; init; }
	public string Reason { get; init; } = null!;
}

public record BanStatus
{
	public DateTime? EndDate { get; init; }
	public string Reason { get; init; } = null!;
	public bool IsBanned { get; init; }
	public bool IsPermanent { get; init; }
}

public record ApproveStatus
{
	public bool IsApproved { get; init; }
}

public record RecentPost // FIXME:
{
	//
}

public record RecentConvo
{
	[JsonProperty("date")] public DateTime CreatedAt { get; init; }
	public string Permalink { get; init; } = null!;
	public string Id { get; init; } = null!; // FIXME:
	public string Subject { get; init; } = null!;
}

public record ConversationUser
{
	public string[] RecentComments { get; init; } = null!;
	public MuteStatus MuteStatus { get; init; } = null!;
	[JsonProperty("name")] public string Username { get; init; } = null!; // FIXME;
	[JsonProperty("date")] public DateTime CreatedAt { get; init; } // FIXME:
	public BanStatus BanStatus { get; init; } = null!;
	public bool IsSuspended { get; init; }
	public ApproveStatus ApproveStatus { get; init; } = null!;
	public bool IsShadowBanned { get; init; }
	public RecentPost[] RecentPosts { get; init; } = null!;
	[JsonProperty("recentConvos")] public Dictionary<string, RecentConvo> RecentConversations { get; init; } = null!;
	[JsonProperty("id")] public string Id36 { get; init; } = null!; // FIXME:
}

public record ConversationModAction
{
	[JsonProperty("date")] public DateTime OccurredAt { get; init; }
	public int ActionTypeId { get; init; } // FIXME:
	public string Id { get; init; } = null!; // FIXME:
	public Author Author = null!;
}

public record GetConversationResponse
{
	public Dictionary<string, Conversation> Conversations = null!;
	public Dictionary<string, ConversationMessage> Messages = null!;
	public Dictionary<string, ConversationUser> User = null!;
	public Dictionary<string, ConversationModAction> ModActions = null!;
}