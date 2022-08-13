using FruityFoundation.Base.Structures;
using SnooBrowser.Browsers;
using SnooBrowser.Models.Comment;
using SnooBrowser.Things;

namespace SnooBrowser.Tests;

public class SubmissionBrowserTests : BrowserTestsBase
{
	private SubmissionBrowser _submissionBrowser = null!;

	[OneTimeSetUp]
	public void Setup()
	{
		_submissionBrowser = GetService<SubmissionBrowser>();
	}

	[Test]
	[TestCase("sphocx", "test post please ignore", @"https://www.reddit.com/r/reddit/comments/sphocx/test_post_please_ignore/", "Go_JasonWaterfalls", true, false, true, "t5_5s5qbl", "reddit")]
	[TestCase("wgg7ay", "Comment Tests", @"https://www.reddit.com/r/SnooBrowserTesting/comments/wgg7ay/comment_tests/", "snoobrowser-testing", false, false, true, "t5_6tjenn", "SnooBrowserTesting")]
	public async Task TestGetValidSubmission(string shortId, string title, string url, string authorName, bool isArchived, bool isLocked, bool isSelfPost, string subredditFullname, string subredditName)
	{
		var resp = await _submissionBrowser.GetSubmission(LinkThing.CreateFromShortId(shortId));
		Assert.That(resp.Try(out var data), Is.True);

		var (submission, comments) = data;

		Assert.That(submission.Title, Is.EqualTo(title));
		Assert.That(submission.Fullname, Is.EqualTo(LinkThing.CreateFromShortId(shortId)));
		Assert.That(submission.Url, Is.EqualTo(url));
		Assert.That(submission.AuthorName, Is.EqualTo(Maybe<string>.Create(authorName)));
		Assert.That(submission.CommentCount, Is.GreaterThan(0)); // We'll never know the right amount, but at least protect against int default
		Assert.That(submission.IsArchived, Is.EqualTo(isArchived));
		Assert.That(submission.IsLocked, Is.EqualTo(isLocked));
		Assert.That(submission.SelfText.OrValue(""), Is.Not.Empty); // Protect against edits and string default
		Assert.That(submission.SubredditFullname, Is.EqualTo(SubredditThing.CreateFromFullId(subredditFullname)));
		Assert.That(submission.Subreddit, Is.EqualTo(subredditName));
		Assert.That(submission.IsSelfPost, Is.EqualTo(isSelfPost));
		Assert.That(submission.RedditPostId, Is.EqualTo(shortId));
		
		Assert.That(comments, Is.Not.Empty);

		var firstComment = comments[0];
		Assert.That(firstComment.IsArchived, Is.EqualTo(isArchived));
		Assert.That(firstComment.IsEdited, Is.False);
		Assert.That(firstComment.BodyAsMarkdown, Is.Not.Empty);
		Assert.That(firstComment.DistinguishType, Is.AnyOf(DistinguishType.None, DistinguishType.Moderator));

	}
}