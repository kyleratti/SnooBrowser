﻿using FruityFoundation.Base.Structures;
using SnooBrowser.Browsers;
using SnooBrowser.Models.Comment;
using SnooBrowser.Models.Subreddit;
using SnooBrowser.Things;

namespace SnooBrowser.Tests;

public class CommentBrowserTests : BrowserTestsBase
{
	// We'll hard code this for now.
	// When support is added for creating submissions, each test should create its own submission to test with.
	private readonly LinkThing _submissionId = LinkThing.CreateFromShortId("wgg7ay");

	private CommentBrowser _commentBrowser = null!;
	private MeBrowser _meBrowser = null!;
	private SubmissionBrowser _submissionBrowser = null!;

	[OneTimeSetUp]
	public void Setup()
	{
		_commentBrowser = GetService<CommentBrowser>();
		_meBrowser = GetService<MeBrowser>();
		_submissionBrowser = GetService<SubmissionBrowser>();
	}

	[Test]
	public async Task TestReplyToSubmission()
	{
		var me = await _meBrowser.GetMe();
		var now = DateTimeOffset.UtcNow.AddSeconds(-5); // The API doesn't give microseconds, so this can cause failures if we don't offset it

		const string markdownText = "This is an automated test";
		var result = await _commentBrowser.SubmitComment(_submissionId, markdownText);
		
		Assert.That(result.AuthorName, Is.EqualTo(me.Username));
		Assert.That(result.AuthorId, Is.EqualTo(AccountThing.CreateFromShortId(me.Id36)));
		Assert.That(result.CommentId, Is.Not.Null);
		Assert.That(result.CreatedAt, Is.AtLeast(now));
		Assert.That(result.DistinguishType, Is.EqualTo(DistinguishType.None));
		Assert.That(result.IsStickied, Is.False);
		Assert.That(result.SubredditId, Is.EqualTo(SubredditThing.CreateFromFullId("t5_6tjenn")));
		Assert.That(result.SubredditName, Is.EqualTo("SnooBrowserTesting"));
		Assert.That(result.SubredditType, Is.EqualTo(SubredditType.Private));
		Assert.That(result.BodyAsMarkdown, Is.EqualTo(markdownText));
		Assert.That(result.BodyAsEncodedHtml, Is.Not.Empty);
	}

	[Test]
	public async Task TestDistinguishCommentAsMod()
	{
		var originalComment = await _commentBrowser.SubmitComment(_submissionId, nameof(TestDistinguishCommentAsMod));
		Assert.That(originalComment.DistinguishType, Is.EqualTo(DistinguishType.None));

		var currentState = await _commentBrowser.DistinguishComment(originalComment.CommentId, DistinguishType.Moderator, isSticky: false);
		Assert.That(currentState.DistinguishType, Is.EqualTo(DistinguishType.Moderator));
	}

	[Test]
	public async Task TestStickyComment()
	{
		var originalComment = await _commentBrowser.SubmitComment(_submissionId, nameof(TestStickyComment));
		Assert.That(originalComment.DistinguishType, Is.EqualTo(DistinguishType.None));
		Assert.That(originalComment.IsStickied, Is.False);

		var currentState = await _commentBrowser.DistinguishComment(originalComment.CommentId, DistinguishType.Moderator, isSticky: true);
		Assert.That(currentState.DistinguishType, Is.EqualTo(DistinguishType.Moderator));
		Assert.That(currentState.IsStickied, Is.True);
	}

	[Test]
	public async Task TestEditComment()
	{
		var comment = await _commentBrowser.SubmitComment(_submissionId, nameof(TestEditComment));
		Assert.That(comment.BodyAsMarkdown, Is.EqualTo(nameof(TestEditComment)));

		const string newCommentText = "I edited this! Yay!";
		var updatedComment = await _commentBrowser.EditComment(comment.CommentId, newCommentText);
		Assert.That(updatedComment.BodyAsMarkdown, Is.EqualTo(newCommentText));
	}

	[Test]
	public async Task TestDeleteComment()
	{
		var comment = await _commentBrowser.SubmitComment(_submissionId, nameof(TestDeleteComment));
		Assert.That(comment.BodyAsMarkdown, Is.EqualTo(nameof(TestDeleteComment)));

		await _commentBrowser.DeleteComment(comment.CommentId);
	}

	[Test]
	public async Task TestLockComment()
	{
		var comment = await _commentBrowser.SubmitComment(_submissionId, nameof(TestLockComment));
		Assert.That(comment.BodyAsMarkdown, Is.EqualTo(nameof(TestLockComment)));

		await _commentBrowser.LockComment(comment.CommentId);

		var updatedComment = (await _submissionBrowser.GetSubmission(_submissionId)).Value.Comments
			.FirstOrEmpty(x => x.CommentId.Equals(comment.CommentId));

		Assert.That(updatedComment.HasValue);
		Assert.That(updatedComment.Value.IsLocked);

		await _commentBrowser.DeleteComment(comment.CommentId);
	}

	[Test]
	public async Task TestUnlockComment()
	{
		var comment = await _commentBrowser.SubmitComment(_submissionId, nameof(TestUnlockComment));
		Assert.That(comment.BodyAsMarkdown, Is.EqualTo(nameof(TestUnlockComment)));

		await _commentBrowser.LockComment(comment.CommentId);

		var updatedComment = (await _submissionBrowser.GetSubmission(_submissionId)).Value.Comments
			.FirstOrEmpty(x => x.CommentId.Equals(comment.CommentId));

		Assert.That(updatedComment.HasValue);
		Assert.That(updatedComment.Value.IsLocked);

		await _commentBrowser.UnlockComment(comment.CommentId);

		var unlockedComment = (await _submissionBrowser.GetSubmission(_submissionId)).Value.Comments
			.FirstOrEmpty(x => x.CommentId.Equals(comment.CommentId));

		Assert.That(unlockedComment.HasValue);
		Assert.That(unlockedComment.Value.IsLocked, Is.False);

		await _commentBrowser.DeleteComment(comment.CommentId);
	}
}