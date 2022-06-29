using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Models;
using SnooBrowser.Models.SubredditModeration;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class SubredditModerationBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public SubredditModerationBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<GetModLogResponse> GetModLog(string subreddit, int limit = 100) =>
		(await _snooBrowserHttpClient.Get<GetModLogResponse>(
			UrlHelper.BuildOAuthUrl($"r/{subreddit}/about/log.json",
				new Dictionary<string, string> { { "limit", limit.ToString(CultureInfo.InvariantCulture) } }))
		)!;

	public async Task<GetModQueueResponse> GetModQueue(string subreddit, int limit = 100) =>
		(await _snooBrowserHttpClient.Get<GetModQueueResponse>(
			UrlHelper.BuildOAuthUrl($"r/{subreddit}/about/modqueue.json",
			new Dictionary<string, string> { { "limit", limit.ToString(CultureInfo.InvariantCulture) } }))
		)!;

	private async Task<bool> RemoveContent(OneOf<CommentThing, LinkThing> thing, bool isSpam)
	{
		var fullId = thing.RawItem switch
		{
			CommentThing ct => ct.FullId,
			LinkThing lt => lt.FullId,
			_ => throw new NotImplementedException($"Unhandled thing: {thing.RawItem?.GetType().FullName}")
		};

		await _snooBrowserHttpClient.Post(UrlHelper.BuildOAuthUrl("api/remove"), MessageBodyType.FormUrlEncoded, new
		{
			id = fullId,
			spam = isSpam
		});

		return true; // FIXME:
	}

	public async Task<bool> RemoveContent(OneOf<CommentThing, LinkThing> thing) =>
		await RemoveContent(thing, isSpam: false);

	public async Task<bool> ApproveContent(OneOf<CommentThing, LinkThing> thing)
	{
		var fullId = thing.RawItem switch
		{
			CommentThing ct => ct.FullId,
			LinkThing lt => lt.FullId,
			_ => throw new NotImplementedException($"Unhandled thing: {thing.RawItem?.GetType().FullName}")
		};

		await _snooBrowserHttpClient.Post(UrlHelper.BuildOAuthUrl("api/approve"), MessageBodyType.FormUrlEncoded, new
		{
			id = fullId
		});

		return true; // FIXME:
	}

	public async Task<bool> IgnoreReports(OneOf<CommentThing, LinkThing> thing)
	{
		var fullId = thing.RawItem switch
		{
			CommentThing ct => ct.FullId,
			LinkThing lt => lt.FullId,
			_ => throw new NotImplementedException($"Unhandled thing: {thing.RawItem?.GetType().FullName}")
		};

		await _snooBrowserHttpClient.Post(UrlHelper.BuildOAuthUrl("api/ignore_reports"), MessageBodyType.FormUrlEncoded, new
		{
			id = fullId,
		});

		return true; // FIXME:
	}

	public async Task<bool> SpamContent(OneOf<CommentThing, LinkThing> thing) =>
		await RemoveContent(thing, isSpam: true);

	public async Task BanUser(string username, (AccountThing Fullname, string DisplayName) subreddit, Maybe<int> duration,
		string modNote, string userMessage)
	{
		if (duration.Try(out var banLength))
		{
			switch (banLength)
			{
				case <= 0:
					throw new ArgumentOutOfRangeException(nameof(duration), banLength,
						"Argument cannot be <= 0. If you want to submit a permanent ban, pass an Empty.");
				case > 999:
					throw new ArgumentOutOfRangeException(nameof(duration), banLength,
						"Argument cannot be > 999. If you want to submit a permanent ban, pass an Empty.");
			}
		}

		var resp =
			await _snooBrowserHttpClient.Post<RedditUiResponse>(UrlHelper.BuildOAuthUrl($"r/{subreddit.DisplayName}/api/friend"),
				MessageBodyType.FormUrlEncoded, new
				{
					action = "add",
					container = subreddit.Fullname.FullId,
					type = FriendActionType.Banned.ToApiName(),
					name = username,
					ban_reason = "", // If set to anything, this prefixes the note property below.
					note = modNote,
					ban_message = userMessage,
					id = "#banned", // This must be #banned. I don't know why.
					ban_context = "", // This is seemingly unused, and although the docs say it should be a Thing's full ID, it doesn't.
					duration = duration.ToNullable(),
					r = subreddit.DisplayName,
					permission = ""
				});

		if (!(resp?.IsSuccess ?? false))
			throw new Exception($"Failed to ban user: {resp}");
	}
}