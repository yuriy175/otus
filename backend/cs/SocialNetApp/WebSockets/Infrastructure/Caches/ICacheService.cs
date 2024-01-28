namespace WebSockets.Infrastructure.Caches
{
    public interface ICacheService
    {
        Task UpsertUserWebSocketAsync(uint userId, string hostName, string port);
    }
}
