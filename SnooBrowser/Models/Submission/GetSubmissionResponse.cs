using FruityFoundation.Base.Structures;
using Newtonsoft.Json;
using SnooBrowser.Things;

namespace SnooBrowser.Models.Submission;

public record GetSubmissionResponse
{
	/// <summary>
	/// The name of the subreddit.
	/// </summary>
	/// <example>"aww"</example>
	public string SubredditName { get; }
	private string? _selfText { get; }
	/// <summary>
	/// The self text of the post.
	/// If the post has no text, this is Empty.
	/// </summary>
	public Maybe<string> SelfText => Maybe<string>.Create(_selfText!, hasValue: !string.IsNullOrEmpty(_selfText));
	/// <summary>
	/// The title of the submission.
	/// </summary>
	public string Title { get; }
	private string _submissionFullname { get; }
	public LinkThing Fullname => LinkThing.CreateFromFullId(_submissionFullname);
	/// <summary>
	/// Whether the submission is a self post or link post.
	/// </summary>
	public bool IsSelfPost { get; }
	/// <summary>
	/// Whether the submission is archived.
	/// An archived submission cannot be replied to, but existing comments under it can be edited or deleted.
	/// </summary>
	public bool IsArchived { get; }
	/// <summary>
	/// Whether the submission is locked.
	/// A locked submission can only be replied to by moderators.
	/// </summary>
	public bool IsLocked { get; }
	private string _subredditFullname { get; }
	public SubredditThing SubredditFullname => SubredditThing.CreateFromFullId(_subredditFullname);
	/// <summary>
	/// The reddit post ID.
	/// </summary>
	/// <example>"vf79a9"</example>
	public string RedditPostId { get; }
	private string? _authorName { get; }

	/// <summary>
	/// The name of the user who submitted the link without any prefixes.
	/// This will be Empty if the author has deleted their account.
	/// </summary>
	/// <example>Maybe&lt;string&gt;("rylekatti") or Maybe&lt;string&gt;.Empty()</example>
	public Maybe<string> AuthorName => Maybe<string>.Create(_authorName!, hasValue: !string.IsNullOrEmpty(_authorName));
	/// <summary>
	/// The number of comments currently posted under this submission.
	/// </summary>
	public int CommentCount { get; }
	/// <summary>
	/// The permalink, relative to reddit.
	/// </summary>
	/// <example>"/r/AskProgramming/comments/vf79a9/are_rich_client_apps_still_a_thing/"</example>
	public string Permalink { get; }
	/// <summary>
	/// The full URL to the submission, including reddit's domain.
	/// </summary>
	/// <example>"https://www.reddit.com/r/AskProgramming/comments/vf79a9/are_rich_client_apps_still_a_thing/"</example>
	public string Url { get; }

	[JsonConstructor]
	public GetSubmissionResponse(
		string subredditName, string? selfText, string title,
		string name, bool is_self, bool archived,
		bool locked, string subreddit_id, string id,
		string? author, int num_comments, string permalink,
		string url
	)
	{
		SubredditName = subredditName;
		_selfText = selfText;
		Title = title;
		_submissionFullname = name;
		IsSelfPost = is_self;
		IsArchived = archived;
		IsLocked = locked;
		_subredditFullname = subreddit_id;
		RedditPostId = id;
		_authorName = author;
		CommentCount = num_comments;
		Permalink = permalink;
		Url = url;
	}
}