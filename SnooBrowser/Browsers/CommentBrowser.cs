using System;
using System.Threading.Tasks;
using SnooBrowser.Models.Comment;
using SnooBrowser.Things;
using SnooBrowser.Util;

namespace SnooBrowser.Browsers;

public class CommentBrowser
{
	private readonly SnooBrowserHttpClient _snooBrowserHttpClient;

	public CommentBrowser(SnooBrowserHttpClient snooBrowserHttpClient)
	{
		_snooBrowserHttpClient = snooBrowserHttpClient;
	}

	public async Task<SubmitCommentResponse> SubmitComment(OneOf<CommentThing, LinkThing, MessageThing> parent, string text)
	{
		var parentThingId = parent.RawItem switch
		{
			CommentThing ct => ct.FullId,
			LinkThing lt => lt.FullId,
			MessageThing mt => mt.FullId,
			_ => throw new ApplicationException($"Unsupported {nameof(ThingType)}: {parent.RawItem?.GetType().FullName}")
		};

		var resp = await _snooBrowserHttpClient.Post<SubmitCommentResponse>(new Uri("api/comment"), MessageBodyType.FormUrlEncoded, new
		{
			api_type = "json",
			text,
			thing_id = parentThingId
		});

		return resp!;
	}
}