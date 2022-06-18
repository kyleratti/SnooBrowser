using System;
using Newtonsoft.Json;

namespace SnooBrowser.Models.Me;

public record UserSubreddit
{
	public bool DefaultSet { get; init; } // FIXME:
	public bool UserIsContributor { get; init; } // FIXME:
	[JsonProperty("banner_img")] public string BannerImage { get; init; } = null!;
	public bool RestrictPosting { get; init; }
	[JsonProperty("free_form_reports")] public bool AllowsFreeFormReports { get; init; }
	public string CommunityIcon { get; init; } = null!;
	public bool ShowMedia { get; init; }
	public string IconColor { get; init; } = null!; // FIXME:
	public bool UserIsMuted { get; init; }
	public string DisplayName { get; init; } = null!; // FIXME:
	[JsonProperty("header_img")] public string HeaderImage { get; init; } = null!;
	public string Title { get; init; } = null!;
	public int Coins { get; init; }
	public string[] PreviousNames { get; init; } = null!;
	public bool Over18 { get; init; }
	public int[] IconSize { get; init; } = null!;
	public string PrimaryColor { get; init; } = null!;
	[JsonProperty("icon_img")] public string IconImage { get; init; } = null!;
	public string Description { get; init; } = null!;
	public string SubmitLinkLabel { get; init; } = null!;
	public string HeaderSize { get; init; } = null!; // FIXME:
	public bool RestrictCommenting { get; init; }
	[JsonProperty("subscribers")] public int SubscriberCount { get; init; }
	public string SubmitTextLabel { get; init; } = null!;
	public bool IsDefaultIcon { get; init; }
	public string LinkFlairPosition { get; init; } = null!;
	public string DisplayNamePrefixed { get; init; } = null!;
	public string KeyColor { get; init; } = null!;
	[JsonProperty("name")] public string Fullname { get; init; } = null!; // FIXME:
	public bool IsDefaultBanner { get; init; }
	public string Url { get; init; } = null!;
	[JsonProperty("quarantine")] public bool IsQuarantined { get; init; }
	public string BannerSize { get; init; } = null!; // FIXME:
	[JsonProperty("is_moderator")] public bool IsModerator { get; init; }
	public string PublicDescription { get; init; } = null!;
	public bool LinkFlairEnabled { get; init; }
	public bool DisableContributorRequests { get; init; }
	public string SubredditType { get; init; } = null!; // FIXME:
	public bool UserIsSubscriber { get; init; }
}

public record UserFeatures
{
	public bool ModServiceMuteWrites { get; init; } // FIXME:
	public bool PromotedTrendBlanks { get; init; } // FIXME:
	public bool ShowAmpLink { get; init; } // FIXME:
	public bool Chat { get; init; } // FIXME:
	public bool IsEmailPermissionRequired { get; init; }
	public bool ModAwards { get; init; } // FIXME:
	public bool ExpensiveCoinsPackage { get; init; } // FIXME:
	public object MwebXpromoRevampV2 { get; init; } = null!; // FIXME:
	public bool AwardsOnStreams { get; init; } // FIXME:
	public bool MwebXpromoModalListingClickDailyDismissibleIos { get; init; } // FIXME:
	public bool ChatSubreddit { get; init; } // FIXME:
	public bool CookieConsentBanner { get; init; } // FIXME:
	public bool ModlogCopyrightRemoval { get; init; } // FIXME:
	public bool ShowNpsSurvey { get; init; } // FIXME:
	public bool DoNotTrack { get; init; } // FIXME:
	public bool ModServiceMuteReads { get; init; } // FIXME:
	public bool ChatUserSettings { get; init; } // FIXME:
	public bool UsePrefAccountDeployment { get; init; } // FIXME:
	public bool MwebXpromoInterstitialCommentsIos { get; init; } // FIXME:
	public bool NoreferrerToNoopener { get; init; } // FIXME:
	public bool PremiumSubscriptionsTable { get; init; } // FIXME:
	public bool MwebXpromoInterstitialCommentsAndroid { get; init; } // FIXME:
	public object MwebNsfwXpromo { get; init; } = null!; // FIXME:
	public bool ChatGroupRollout { get; init; } // FIXME:
	public bool ResizedStylesImages { get; init; } // FIXME:
	public bool SpezModal { get; init; } // FIXME:
	public bool MwebXpromoModalListingClickDailyDismissibleAndroid { get; init; } // FIXME:
}

public record GetMeResponse
{
	public bool IsEmployee { get; init; }
	[JsonProperty("seen_layout_switch")] public bool HasSeenLayoutSwitch { get; init; }
	public bool HasVisitedNewProfile { get; init; }
	[JsonProperty("pref_no_profanity")] public bool PrefersNoProfanity { get; init; }
	public bool HasExternalAccount { get; init; }
	[JsonProperty("pref_geopopular")] public string GeoPopularPreference { get; init; } = null!; // FIXME:
	[JsonProperty("seen_redesign_modal")] public bool HasSeenRedesignModal { get; init; }
	[JsonProperty("pref_show_trending")] public bool ShowTrendingPreference { get; init; }
	[JsonProperty("subreddit")] public UserSubreddit UserSubreddit { get; init; } = null!;
	[JsonProperty("pref_show_presence")] public bool ShowPresencePreference { get; init; }
	[JsonProperty("snoovatar_img")] public string SnoovatarImage { get; init; } = null!;
	public string SnoovatarSize { get; init; } = null!; // FIXME:
	[JsonProperty("gold_expiration")] public DateTime? GoldExpirationDate { get; init; }
	public bool HasGoldSubscription { get; init; }
	public bool IsSponsor { get; init; }
	[JsonProperty("num_friends")] public int FriendCount { get; init; }
	public UserFeatures Features { get; init; } = null!;
	public bool CanEditName { get; init; }
	[JsonProperty("verified")] public bool IsVerified { get; init; }
	public bool NewModmailExists { get; init; } // FIXME:
	[JsonProperty("pref_autoplay")] public bool AutoplayPreference { get; init; }
	public int Coins { get; init; }
	public bool HasPaypalSubscription { get; init; }
	public bool HasSubscribedToPremium { get; init; }
	[JsonProperty("id")] public string Id36 { get; init; } = null!;
	public bool HasStripeSubscription { get; init; }
	public string OauthClientId { get; init; } = null!;
	public bool CanCreateSubreddit { get; init; }
	[JsonProperty("over_18")] public bool IsOver18 { get; init; }
	public bool IsGold { get; init; }
	public bool IsMod { get; init; }
	public int AwarderKarma { get; init; }
	public long? SuspensionExpirationUtc { get; init; }
	public bool HasVerifiedEmail { get; init; }
	public bool IsSuspended { get; init; }
	[JsonProperty("pref_video_autoplay")] public bool VideoAutoplayPreference { get; init; }
	[JsonProperty("in_chat")] public bool IsInChat { get; init; }
	public bool HasAndroidSubscription { get; init; }
	[JsonProperty("in_redesign_beta")] public bool IsInRedesignBeta { get; init; }
	[JsonProperty("icon_img")] public string IconImage { get; init; } = null!;
	public bool HasModMail { get; init; }
	[JsonProperty("pref_nightmode")] public bool NightmodePreference { get; init; }
	public int AwardeeKarma { get; init; }
	public bool HideFromRobots { get; init; } // FIXME:
	public bool PasswordSet { get; init; } // FIXME:
	public int LinkKarma { get; init; }
	public bool ForcePasswordReset { get; init; } // FIXME:
	public int TotalKarma { get; init; }
	public bool SeenGiveAwardTooltip { get; init; } // FIXME:
	public int InboxCount { get; init; } // FIXME:
	public bool SeenPremiumAdblockModal { get; init; } // FIXME:
	public bool PrefTopKarmaSubreddits { get; init; } // FIXME:
	public bool HasMail { get; init; }
	public bool PrefShowSnoovatar { get; init; } // FIXME:
	[JsonProperty("name")] public string Username { get; init; } = null!; // FIXME:
	public int PrefClickgadget { get; init; } // FIXME:
	[JsonProperty("created")] public long CreatedAt { get; init; }
	public int GoldCreddits { get; init; } // FIXME:
	[JsonProperty("created_utc")] public long CreatedAtUtc { get; init; }
	public bool HasIosSubscription { get; init; }
	public bool PrefShowTwitter { get; init; } // FIXME:
	public bool InBeta { get; init; } // FIXME:
	public int CommentKarma { get; init; }
	public bool AcceptFollowers { get; init; } // FIXME:
	public bool HasSubscribed { get; init; } // FIXME:
	public string[] LinkedIdentifies { get; init; } = null!; // FIXME:
	public bool SeenSubredditChatFtux { get; init; } // FIXME:
}