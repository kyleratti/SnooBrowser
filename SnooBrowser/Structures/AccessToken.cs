using System;

namespace SnooBrowser.Structures
{
    public record AccessToken(
        string Token,
        DateTime ExpiresAt
    )
    {
        public bool IsExpired =>
            DateTime.Now > ExpiresAt;
    }
}
