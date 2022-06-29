using FruityFoundation.Base.Structures;
using Microsoft.Extensions.DependencyInjection;
using SnooBrowser.Browsers;
using SnooBrowser.Models;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Tests;

public class SubmissionBrowserTests
{
	private IServiceScope _serviceScope = null!;
	private SubmissionBrowser _submissionBrowser = null!;

	[OneTimeSetUp]
	public void Setup()
	{
		var services = new ServiceCollection();
		services.AddSnooBrowserClient<EnvironmentAuthParameterProvider, InMemoryAccessTokenProvider>();

		var provider = services.BuildServiceProvider();
		_serviceScope = provider.CreateScope();

		_submissionBrowser = provider.GetRequiredService<SubmissionBrowser>();
	}

	[OneTimeTearDown]
	public void TearDown()
	{
		_serviceScope.Dispose();
	}

	[Test]
	public async Task TestGetValidSubmission()
	{
		var resp = await _submissionBrowser.GetSubmission(LinkThing.CreateFromShortId("sphocx"));
		Assert.That(resp.Try(out var data), Is.True);

		var (submission, comments) = data;

		Assert.That(submission.Title, Is.EqualTo("test post please ignore"));
		Assert.That(submission.Fullname, Is.EqualTo(LinkThing.CreateFromShortId("sphocx")));
		Assert.That(submission.Url, Is.EqualTo(@"https://www.reddit.com/r/reddit/comments/sphocx/test_post_please_ignore/"));
		Assert.That(submission.AuthorName, Is.EqualTo(Maybe<string>.Create(@"Go_JasonWaterfalls")));
		Assert.That(submission.CommentCount, Is.GreaterThan(0)); // We'll never know the right amount, but at least protect against int default
		Assert.That(submission.IsArchived, Is.False);
		Assert.That(submission.IsLocked, Is.False);
		Assert.That(submission.SelfText.OrValue(""), Is.Not.Empty); // Protect against edits and string default
		Assert.That(submission.SubredditFullname, Is.EqualTo(SubredditThing.CreateFromFullId("t5_5s5qbl")));
		Assert.That(submission.Subreddit, Is.EqualTo("reddit"));
		Assert.That(submission.IsSelfPost, Is.True);
		Assert.That(submission.RedditPostId, Is.EqualTo("sphocx"));
		
		Assert.That(comments, Is.Not.Empty);

		var firstComment = comments[0];
		Assert.That(firstComment.Fullname, Is.EqualTo(CommentThing.CreateFromShortId("hwfc0a9")));
		Assert.That(firstComment.IsArchived, Is.False);
		Assert.That(firstComment.IsEdited, Is.False);
		Assert.That(firstComment.IsStickied, Is.False);
		Assert.That(firstComment.Body, Is.Not.Empty);
		Assert.That(firstComment.CommentLinkFullname, Is.EqualTo(LinkThing.CreateFromShortId("sphocx")));
		Assert.That(firstComment.AuthorFullname, Is.EqualTo(AccountThing.CreateFromShortId("6sobn")));
		Assert.That(firstComment.DistinguishType, Is.EqualTo(Maybe<DistinguishedCommentType>.Empty()));

	}
}