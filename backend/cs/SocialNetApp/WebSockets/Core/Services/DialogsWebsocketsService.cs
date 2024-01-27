using Common.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Data.SqlTypes;
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
    readonly record struct WebSock(uint? BuddyId, WebSocket Socket, Task? Receiver);

    public class DialogsWebsocketsService : IWebsocketsService
    {
        private readonly ConcurrentDictionary<uint, WebSock> _websockets = new ConcurrentDictionary<uint, WebSock> { };

        private readonly IMQReceiver _mqReceiver;
        private readonly IMQSender _mqSender;
        public DialogsWebsocketsService(IFriendsRepository friendsRepository, IMQReceiver mqReceiver, IMQSender mqSender)
        {
            _mqReceiver = mqReceiver;
            _mqSender = mqSender;
            _mqReceiver.CreateDialogReceiver(async (data) =>
            {
                var text = Encoding.UTF8.GetString(data);
                var message = JsonSerializer.Deserialize<Message>(text);
                var buddyId = Convert.ToUInt32(message.UserId);
                var authorId = Convert.ToUInt32(message.AuthorId);
                var text2 = message.Text;
                Console.WriteLine($"pre [x] WS sent '{buddyId}':'{text2}'");
                //byte[] bytes = Encoding.UTF8.GetBytes($"{id}: {text}");
                if (_websockets.TryGetValue(buddyId, out WebSock ws) && ws.Socket is not null)
                {
                    //a user receives a message from the buddy from the current dialog
                    if (authorId == ws.BuddyId)
                    {
                        //send current dialog message
                        await ws.Socket.SendAsync(
                                data,
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                    }
                    else
                    {
                        // increase counters
                        _mqSender.SendUnreadDialogMessageIds(buddyId, true, new[] { Convert.ToInt32(message.Id) });
                    }
                }
                else
                {
                    // increase counters
                    _mqSender.SendUnreadDialogMessageIds(buddyId, true, new[] { Convert.ToInt32(message.Id) });
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
                var webSock = new WebSock
                {
                    BuddyId = null,
                    Socket = webSocket,
                    Receiver = Task.Run(async () => {
                        var buffSize = 100;
                        var temporaryBuffer = new byte[buffSize];
                        while (true)
                        {
                            _websockets.TryGetValue(userId.Value, out WebSock ws);
                            if(webSocket.State != WebSocketState.Open)
                            {
                                break;
                            }
                            var result = await webSocket.ReceiveAsync(temporaryBuffer, CancellationToken.None);
                            if (result == null || result.CloseStatus == WebSocketCloseStatus.EndpointUnavailable)
                            {
                                break;
                            }
                            var text = Encoding.UTF8.GetString(temporaryBuffer);
                            var buddyId = Convert.ToUInt32(text);
                            
                            _websockets.TryUpdate(userId.Value, new WebSock
                            {
                                BuddyId = buddyId,
                                Socket = ws.Socket,
                                Receiver = ws.Receiver
                            },ws);
                            temporaryBuffer = new byte[buffSize];
                        }
                    }),
                };
                _websockets.AddOrUpdate(
                    Convert.ToUInt32(userId), i => webSock, (i, w) => webSock);

                return task.Task!;
            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
            }
        }
    }
}
