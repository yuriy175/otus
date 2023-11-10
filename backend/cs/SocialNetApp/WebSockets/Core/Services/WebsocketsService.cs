using Common.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Websockets.Core.Model.Interfaces;
using WebSockets.Infrastructure.Repositories.Interfaces;

namespace WebSockets.Core.Services
{
    public class WebsocketsService : IWebsocketsService
    {
        private readonly ConcurrentDictionary<uint, List<WebSocket>> _websockets = new ConcurrentDictionary<uint, List<WebSocket>> { };

        private readonly IFriendsRepository _friendsRepository;
        private readonly IMQReceiver _mqReceiver;
        public WebsocketsService(IFriendsRepository friendsRepository, IMQReceiver mqReceiver)
        {
            _mqReceiver = mqReceiver;
            _friendsRepository = friendsRepository;
            _mqReceiver.CreateReceiver();
        }

        public async Task<Task> OnWebSocketConnectAsync(HttpContext context)
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
            var friends = (await _friendsRepository.GetFriendIdsAsync(userId.Value, cancellationToken.Token)).ToList();
            foreach (var friendId in friends)
            {
                _websockets.AddOrUpdate(
                    Convert.ToUInt32(friendId),
                    id => new List<WebSocket> { webSocket },
                    (id, list) =>
                    {
                        list.Add(webSocket);
                        return list;
                    });
                _mqReceiver.ReceivePosts((uint)friendId, async (id, post) =>
                {
                    Console.WriteLine($"pre [x] WS sent '{id}':'{post}'");
                    byte[] bytes = Encoding.UTF8.GetBytes($"{id}: {post}");
                    if (_websockets.TryGetValue(id, out List<WebSocket>? values) && values is not null)
                    {
                        Console.WriteLine($"pre2 [x] WS sent '{id}':'{post}'");
                        foreach (var ws in values.ToImmutableList())
                        {
                            Console.WriteLine($"pre3 [x] WS sent '{id}':'{post}'");
                            await ws.SendAsync(
                                    bytes,
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
                            Console.WriteLine($" [x] WS sent '{id}':'{post}'");
                        }
                    }
                    //await webSocket.SendAsync(
                    //    bytes,
                    //    WebSocketMessageType.Text,
                    //    true,
                    //    CancellationToken.None);
                });
            }

            return task.Task!;
        }
    }
}
