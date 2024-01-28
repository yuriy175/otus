namespace Dialogs.Infrastructure.Caches
{
    public interface ICacheService
    {
        Task<string> GetUserWebSocketAddressAsync(uint userId);
    }
}
