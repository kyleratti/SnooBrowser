using System.Collections.Generic;
using FruityFoundation.Base.Structures;
using SnooBrowser.Things;

namespace SnooBrowser.Models.Submission;

public record Submission(
	string Subreddit,
	Maybe<string> SelfText,
	string Title,
	LinkThing Fullname,
	bool IsSelfPost,
	bool IsArchived,
	bool IsLocked,
	SubredditThing SubredditFullname,
	string RedditPostId,
	Maybe<string> AuthorName,
	int TotalCommentCount,
	string Permalink,
	string FullUrl,
	IReadOnlyList<Comment.Comment> Comments);