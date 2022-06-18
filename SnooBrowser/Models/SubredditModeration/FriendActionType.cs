using System;

namespace SnooBrowser.Models.SubredditModeration;

public enum FriendActionType
{
	Friend = 1,
	Moderator = 2,
	ModeratorInvite = 3,
	Contributor = 4,
	Banned = 5,
	Muted = 6,
	WikiBanned = 7,
	WikiContributor = 8
}

public static class FriendActionTypeExtensions
{
	public static string ToApiName(this FriendActionType actionType) =>
		actionType switch
		{
			FriendActionType.Friend => "friend",
			FriendActionType.Moderator => "moderator",
			FriendActionType.ModeratorInvite => "moderator_invite",
			FriendActionType.Contributor => "contributor",
			FriendActionType.Banned => "banned",
			FriendActionType.Muted => "muted",
			FriendActionType.WikiBanned => "wikibanned",
			FriendActionType.WikiContributor => "wikicontributor",
			_ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null)
		};
}