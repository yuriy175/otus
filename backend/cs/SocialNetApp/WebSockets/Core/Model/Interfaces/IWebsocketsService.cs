namespace Websockets.Core.Model.Interfaces
{
    public interface IWebsocketsService
    {
        Task OnWebSocketConnectAsync(HttpContext context);
    }
}
