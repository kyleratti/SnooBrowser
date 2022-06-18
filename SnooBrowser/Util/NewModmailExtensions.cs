using System;
using SnooBrowser.Models;

namespace SnooBrowser.Util
{
    public static class NewModmailExtensions
    {
        public static string ToName(this ModmailSort sort) =>
            sort switch
            {
                ModmailSort.Recent => "recent",
                ModmailSort.Mod => "mod",
                ModmailSort.User => "user",
                ModmailSort.Unread => "unread",
                _ => throw new NotImplementedException($"Unsupported sort type: {sort}")
            };

        public static string ToName(this ModmailState state) =>
            state switch
            {
                ModmailState.Default => "default",
                ModmailState.All => "all",
                ModmailState.Appeals => "appeals",
                ModmailState.Notifications => "notifications",
                ModmailState.Inbox => "inbox",
                ModmailState.Filtered => "filtered",
                ModmailState.InProgress => "inprogress",
                ModmailState.Mod => "mod",
                ModmailState.Archived => "archived",
                ModmailState.Highlighted => "highlighted",
                ModmailState.JoinRequests => "join_requests",
                ModmailState.New => "new",
                _ => throw new NotImplementedException($"Unsupported state: {state}")
            };
    }
}
