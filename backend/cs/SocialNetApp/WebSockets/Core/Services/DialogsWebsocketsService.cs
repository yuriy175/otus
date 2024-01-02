using Common.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using Websockets.Core.Model;
using Websockets.Core.Model.Interfaces;
using WebSockets.Infrastructure.Repositories.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebSockets.Core.Services
{
    public class DialogsWebsocketsService : IWebsocketsService
    {
        private readonly ConcurrentDictionary<uint, WebSocket> _websockets = new ConcurrentDictionary<uint, WebSocket> { };

        private readonly IMQReceiver _mqReceiver;
        public DialogsWebsocketsService(IFriendsRepository friendsRepository, IMQReceiver mqReceiver)
        {
            _mqReceiver = mqReceiver;
            _mqReceiver.CreateDialogReceiver(async (data) =>
            {
                var text = Encoding.UTF8.GetString(data);
                var message = JsonSerializer.Deserialize<Message>(text);
                var id = Convert.ToUInt32(message.UserId);
                var text2 = message.Text;
                Console.WriteLine($"pre [x] WS sent '{id}':'{text2}'");
                //byte[] bytes = Encoding.UTF8.GetBytes($"{id}: {text}");
                if (_websockets.TryGetValue(id, out WebSocket? ws) && ws is not null)
                {
                    await ws.SendAsync(
                            data, //bytes,
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                }
            });
        }

        public async Task<Task> OnWebSocketConnectAsync(HttpContext context)
        {
            try
            {
                var task = new TaskCompletionSource();
                var token = context.Request.Query["token"];
                var userId = AuthUtils.GetAuthorizedUserId(token);
                if (userId == null)
                {
                    return Task.FromException(new Exception());
                }
                var cancellationToken = new CancellationTokenSource();
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                _websockets.AddOrUpdate(
                    Convert.ToUInt32(userId), i => webSocket, (i, w) => webSocket);
                return task.Task!;
            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
                var y = 0;
            }
        }
    }
}
