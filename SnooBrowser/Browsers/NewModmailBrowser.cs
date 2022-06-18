using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FruityFoundation.Base.Structures;
using SnooBrowser.Models;
using SnooBrowser.Models.NewModmail;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class NewModmailBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public NewModmailBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public Task<GetConversationsResponse> GetConversations(string subreddit, ModmailSort sort,
		ModmailState state, int limit = 25, Maybe<string> after = default) =>
		GetConversations(new[] { subreddit }, sort, state, limit, after);

	public async Task<GetConversationsResponse> GetConversations(string[] subreddits, ModmailSort sort,
		ModmailState state, int limit = 25, Maybe<string> after = default)
	{
		var queryParams = new Dictionary<string, string>
		{
			{ "entity", string.Join(',', subreddits) },
			{ "limit", limit.ToString(CultureInfo.InvariantCulture) },
			{ "sort", sort.ToName() },
			{ "state", state.ToName() }
		};
		if (after.HasValue)
			queryParams.Add("after", after.Value);

		return (await _snooBrowserHttpClient.Get<GetConversationsResponse>(UrlHelper.Build("api/mod/conversations",
			queryParams)))!;
	}

	public async Task<bool> ArchiveConversation(ConversationId convoId)
	{
		await _snooBrowserHttpClient.Post(UrlHelper.Build($"api/mod/conversations/{convoId.RawId}/archive"));
		return true; // FIXME:
	}

	public async Task<GetConversationResponse> GetConversation(ConversationId convoId) =>
		(await _snooBrowserHttpClient.Get<GetConversationResponse>(UrlHelper.Build($"api/mod/conversations/{convoId.RawId}")))!;
}