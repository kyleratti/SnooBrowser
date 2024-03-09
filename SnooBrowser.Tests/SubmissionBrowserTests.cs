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

		Assert.That(data.Title, Is.EqualTo(title));
		Assert.That(data.Fullname, Is.EqualTo(LinkThing.CreateFromShortId(shortId)));
		Assert.That(data.FullUrl, Is.EqualTo(url));
		Assert.That(data.AuthorName, Is.EqualTo(Maybe.Just(authorName)));
		Assert.That(data.TotalCommentCount, Is.GreaterThan(0)); // We'll never know the right amount, but at least protect against int default
		Assert.That(data.IsArchived, Is.EqualTo(isArchived));
		Assert.That(data.IsLocked, Is.EqualTo(isLocked));
		Assert.That(data.SelfText.OrValue(""), Is.Not.Empty); // Protect against edits and string default
		Assert.That(data.SubredditFullname, Is.EqualTo(SubredditThing.CreateFromFullId(subredditFullname)));
		Assert.That(data.Subreddit, Is.EqualTo(subredditName));
		Assert.That(data.IsSelfPost, Is.EqualTo(isSelfPost));
		Assert.That(data.RedditPostId, Is.EqualTo(shortId));
		
		Assert.That(data.Comments, Is.Not.Empty);

		var firstComment = data.Comments[0];
		Assert.That(firstComment.IsArchived, Is.EqualTo(isArchived));
		Assert.That(firstComment.IsEdited, Is.False);
		Assert.That(firstComment.BodyAsMarkdown, Is.Not.Empty);
		Assert.That(firstComment.DistinguishType, Is.AnyOf(DistinguishType.None, DistinguishType.Moderator));
	}

	[Test]
	public async Task TestInvalidLinkIdIsHandled()
	{
		var result = await _submissionBrowser.GetSubmission(LinkThing.CreateFromShortId(@"123"));

		Assert.That(result.HasValue, Is.False);
	}
}
