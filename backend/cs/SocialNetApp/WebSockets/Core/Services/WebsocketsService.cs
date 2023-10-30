using Common.Core.Model;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Websockets.Core.Model.Interfaces;
using WebSockets.Infrastructure.Repositories.Interfaces;

namespace WebSockets.Core.Services
{
    public class WebsocketsService : IWebsocketsService
    {
        private readonly ConcurrentDictionary<uint, WebSocket> _websockets = new ConcurrentDictionary<uint, WebSocket> { };

        private readonly IFriendsRepository _friendsRepository;

        public WebsocketsService(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        public async Task OnWebSocketConnectAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            var userId = AuthUtils.GetAuthorizedUserId(token);
            if (userId == null)
            {
                return;
            }
            var cancellationToken = new CancellationTokenSource();
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            {
                var friends = await _friendsRepository.GetFriendIdsAsync(userId.Value, cancellationToken.Token);
                var i = 0;
                while (true)
                {
                    //var buffer = new byte[1024 * 4];
                    //var receiveResult = await webSocket.ReceiveAsync(
                    //    new ArraySegment<byte>(buffer), CancellationToken.None);
                    //await webSocket.SendAsync(
                    //    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    //    receiveResult.MessageType,
                    //    receiveResult.EndOfMessage,
                    //    CancellationToken.None);
                    byte[] bytes = Encoding.UTF8.GetBytes("Фыва qwer " + (i++).ToString() + $" {friends?.FirstOrDefault()}");

                    await Task.Delay(500);
                    await webSocket.SendAsync(
                        bytes,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
        }
    }
}
