namespace Websockets.Core.Model.Interfaces
{
    public interface IWebsocketsService
    {
        Task<Task> OnWebSocketConnectAsync(HttpContext context);
    }
}
