namespace SnooBrowser.Interfaces.Auth
{
    public record GetAccessTokenResponse
    {
        public string AccessToken = null!;
        public string TokenType = null!; // FIXME:
        /// <summary>
        /// The time, in Unix Epoch seconds, at which the <see cref="AccessToken"/> expires.
        /// </summary>
        public long ExpiresIn;
        public string Scope = null!;
        public string RefreshToken = null!; // FIXME: should be a Maybe
    }
}
