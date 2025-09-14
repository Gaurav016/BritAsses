using System.Collections.Concurrent;

public class InMemoryRefreshTokenStore
{
    private readonly ConcurrentDictionary<string, string> _refreshTokens = new();

    public void SaveRefreshToken(string username, string refreshToken)
    {
        _refreshTokens[username] = refreshToken;
    }

    public bool ValidateRefreshToken(string username, string refreshToken)
    {
        return _refreshTokens.TryGetValue(username, out var storedToken) && storedToken == refreshToken;
    }

    public void RemoveRefreshToken(string username)
    {
        _refreshTokens.TryRemove(username, out _);
    }
}
