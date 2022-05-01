namespace SnooBrowser.Structures
{
    public record AuthParameters(
        string AppId,
        string AppSecret,
        string RefreshToken
    );
}
