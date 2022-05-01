using FruityFoundation.Base.Structures;
using Newtonsoft.Json;
using SnooBrowser.Things;

namespace SnooBrowser.Structures.SubredditModeration
{
    public record ModQueueEntryData
    {
        public object[] Awarders { get; init; } = null!;
        [JsonProperty("subreddit_id")] public string SubredditFullnameAsString { get; init; } = null!;
        public Fullname SubredditFullname => Fullname.FromString(SubredditFullnameAsString);
        [JsonProperty("approved_at_utc")] public long? ApprovedAtUtc { get; init; }
        [JsonProperty("author_is_blocked")] public bool IsAuthorBlocked { get; init; }
        public string CommentType { get; init; } = null!;
        [JsonProperty("edited")] public bool IsEdited { get; init; }
        public string ModReasonBy { get; init; } = null!;
        public string BannedBy { get; init; } = null!;
        public string AuthorFlairText { get; init; } = null!;
        public int TotalAwardsReceived { get; init; }
        /// <summary>
        /// The name of the subreddit without the /r/ prefix.
        /// </summary>
        /// <example>"PublicFreakout"</example>
        [JsonProperty("subreddit")] public string SubredditName { get; init; } = null!;
        [JsonProperty("removed")] public bool IsRemoved { get; init; }
        /// <summary>
        /// The name of the user who submitted the reported content without the /u/ prefix.
        /// </summary>
        /// <example>"AutoModerator"</example>
        [JsonProperty("author")] public string AuthorUsername { get; init; } = null!;
        public object Likes { get; init; } = null!;
        public string Replies { get; init; } = null!;
        // FIXME: what are the two bools??? WHY DOES REDDIT NOT DOCUMENT ANY OF THIS
        //[JsonProperty("user_reports")] public (string RuleName, int ReportCount, bool, bool IsCustom)[] UserReports { get; init; }
        [JsonProperty("user_reports")] public object[] UserReports { get; init; } = null!;
        [JsonProperty("saved")] public bool IsSaved { get; init; }
        /// <summary>
        /// The ID of this item.
        /// </summary>
        /// <example>"hb90l7w"</example>
        [JsonProperty("id")] public string IdAsString { get; init; } = null!;
        [JsonProperty("name")] public string NameAsString { get; init; } = null!;
        public Fullname NameAsFullname => Fullname.FromString(NameAsString);
        public bool IsLink => NameAsFullname.IsLink;
        public bool IsComment => NameAsFullname.IsComment;
        public long? BannedAtUtc { get; init; }
        public string ModReasonTitle { get; init; } = null!;
        public int Guilded { get; init; }
        [JsonProperty("archived")] public bool IsArchived { get; init; }
        public string CollapsedReasonCode { get; init; } = null!;
        [JsonProperty("no_follow")] public bool IsNoFollow { get; init; } // FIXME: is this no follow links?
        [JsonProperty("spam")] public bool IsSpam { get; init; }
        /// <summary>
        /// The total number of comments on the link.
        /// </summary>
        [JsonProperty("num_comments")] public int CommentCount { get; init; }
        public string[] TreatmentTags { get; init; } = null!;
        public bool CanModPost { get; init; }
        [JsonProperty("ignore_reports")] public bool IsIgnoreReports { get; init; }
        [JsonProperty("send_replies")] public bool CanSendReplies { get; init; }
        /// <summary>
        /// The Fullname of the parent if this reported item is a comment.
        /// This is <c>null</c> if the report is a submission.
        /// </summary>
        [JsonProperty("parent_id")] public string ParentAsString { get; init; } = null!;

        public Maybe<Fullname> Parent => 
            Maybe<string>
                .Create(ParentAsString, string.IsNullOrEmpty)
                .Map(Fullname.FromString);
        [JsonProperty("score")] public int Karma { get; init; }
        [JsonProperty("author_fullname")] public string AuthorFullnameAsString { get; init; } = null!;
        public Fullname AuthorFullname => Fullname.FromString(AuthorFullnameAsString);
        [JsonProperty("over_18")] public bool IsNsfw { get; init; }
        public string RemovalReason { get; init; } = null!;
        public string ApprovedBy { get; init; } = null!;
        public string ModNote { get; init; } = null!;
        [JsonProperty("controversiality")] public int Controversiality { get; init; }
        [JsonProperty("collapsed")] public bool IsCollapsed { get; init; }
        [JsonProperty("body")] public string BodyAsMarkdown { get; init; } = null!;
        /// <summary>
        /// The title of the comment's parent link.
        /// </summary>
        public string LinkTitle { get; init; } = null!;
        [JsonProperty("title")] public string TitleAsString { get; init; } = null!;
        public object TopAwardedType { get; init; } = null!;
        public string AuthorFlairCssClass { get; init; } = null!;
        public string AuthorPatreonFlair { get; init; } = null!;
        [JsonProperty("downs")] public int Downvotes { get; init; } // FIXME: is this right?
        [JsonProperty("author_flair_richtext")] public object[] AuthorFlairRichtext { get; init; } = null!;
        [JsonProperty("is_submitter")] public bool IsSubmitter { get; init; }
        [JsonProperty("body_html")] public string BodyAsHtml { get; init; } = null!;
        public object Gildings { get; init; } = null!;
        public string CollapsedReason { get; init; } = null!;
        public object AssociatedAward { get; init; } = null!;
        [JsonProperty("stickied")] public bool IsStickied { get; init; }
        [JsonProperty("author_premium")] public bool IsAuthorPremium { get; init; }
        [JsonProperty("can_guild")] public bool CanGuild { get; init; }
        [JsonProperty("link_id")] public string LinkIdAsString { get; init; } = null!;

        /// <summary>
        /// The Fullname of the link the reported comment belongs to.
        /// If the reported content is a submission, this is Empty.
        /// </summary>
        public Maybe<Fullname> LinkId =>
            Maybe<string>
                .Create(LinkIdAsString, string.IsNullOrEmpty)
                .Map(Fullname.FromString);
        [JsonProperty("approved")] public bool IsApproved { get; init; }
        public object AuthorFlairTextColor { get; init; } = null!;
        public object[] AllAwardings { get; init; } = null!;
        [JsonProperty("score_hidden")] public bool IsScoreHidden { get; init; }
        /// <summary>
        /// Link to the reported content, relative to reddit's domain.
        /// </summary>
        /// <example>/r/PublicFreakout/comments/pf7j6p/when_your_mother_is_a_reporter_and_your_father_is/hb90l7w/</example>
        public string Permalink { get; init; } = null!;
        /// <example>"public"</example>
        public string SubredditType { get; init; } = null!;
        /// <summary>
        /// The full link to the reported content's parent link.
        /// </summary>
        /// <example>"https://www.reddit.com/r/PublicFreakout/comments/pf7j6p/when_your_mother_is_a_reporter_and_your_father_is/"</example>
        [JsonProperty("link_permalink")] public string LinkPermalink { get; init; } = null!;
        [JsonProperty("created")] public long CreatedAt { get; init; }
        [JsonProperty("created_utc")] public long CreatedAtUtc { get; init; }
        /// <summary>
        /// The name of the subreddit with the r/ prefix.
        /// </summary>
        /// <example>r/PublicFreakout</example>
        [JsonProperty("subreddit_name_prefixed")] public string SubredditNamePrefixed { get; init; } = null!;
        [JsonProperty("ups")] public int UpvoteCount { get; init; }
        [JsonProperty("locked")] public bool IsLocked { get; init; }
        [JsonProperty("author_flair_background_color")] public object AuthorFlairBackgroundColor { get; init; } = null!;
        [JsonProperty("collapsed_because_crowd_control")] public object CollapsedBecauseCrowdControl { get; init; } = null!;
        [JsonProperty("mod_reports")] public (string ReportReason, string ModUsername)[] ModReports { get; init; } = null!;
        [JsonProperty("quarantine")] public bool IsSubredditQuarantined { get; init; }
        [JsonProperty("num_reports")] public int TotalReportCount { get; init; }
        public object Distinguished { get; init; } = null!;
        public object AuthorFlairTemplateId { get; init; } = null!;
    }

    public record ModQueueEntry
    {
        public string Kind { get; init; } = null!;
        [JsonProperty("data")] public ModQueueEntryData Data { get; init; } = null!;
    }

    public record ModQueueListingData
    {
        [JsonProperty("after")] public string AfterAsString { get; init; } = null!;

        public Fullname AfterFullname => Fullname.FromString(AfterAsString);
        public int Dist { get; init; }
        [JsonProperty("geo_filter")] public string GeoFilter { get; init; } = null!;
        [JsonProperty("children")] public ModQueueEntry[] Entries { get; init; } = null!;
    }

    public record GetModQueueResponse
    {
        public string Kind { get; init; } = null!;
        public ModQueueListingData Data { get; init; } = null!;
    }
}
