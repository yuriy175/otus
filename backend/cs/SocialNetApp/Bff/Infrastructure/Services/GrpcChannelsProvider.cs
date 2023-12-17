using Bff.Infrastructure.Services.Interfaces;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Bff.Infrastructure.gRpc.Services
{
    public class GrpcChannelsProvider : IGrpcChannelsProvider
    {
        private readonly static string _grpcPostsUrl = Environment.GetEnvironmentVariable("GRPC_POSTS");
        private readonly static string _grpcUsersUrl = Environment.GetEnvironmentVariable("GRPC_PROFILE");
        private readonly static string _grpcDialogsUrl = Environment.GetEnvironmentVariable("GRPC_DIALOGS");

        private GrpcChannel _authChannel = null;
        private GrpcChannel _usersChannel = null;
        private GrpcChannel _dialogsChannel = null;
        private GrpcChannel _postsChannel = null;

        private readonly GrpcChannelOptions _options = null;
        private readonly object _authLock = new object();
        private readonly object _usersLock = new object();
        private readonly object _dialogsLock = new object();
        private readonly object _postsLock = new object();

        public GrpcChannelsProvider()
        {
            _options = new GrpcChannelOptions()
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                }
            };
        }

        public GrpcChannel GetAuthChannel() => CreateChannelAsync(ref _authChannel, _grpcUsersUrl, _authLock);
        public GrpcChannel GetUsersChannel() => CreateChannelAsync(ref _usersChannel, _grpcUsersUrl, _usersLock);
        public GrpcChannel GetDialogsChannel() => CreateChannelAsync(ref _dialogsChannel, _grpcDialogsUrl, _dialogsLock);
        public GrpcChannel GetPostsChannel() => CreateChannelAsync(ref _postsChannel, _grpcPostsUrl, _postsLock);

        private GrpcChannel CreateChannelAsync(ref GrpcChannel channel, string url, object locker)
        {
            lock (locker)
            {
                if(channel?.State == Grpc.Core.ConnectivityState.Ready)
                {
                    return channel;
                }
                channel = GrpcChannel.ForAddress(url, _options);
                channel.ConnectAsync().Wait();

                return channel;
            }
        }
    }
}
