using Bff.Infrastructure.Services.Interfaces;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Bff.Infrastructure.gRpc.Services
{
    public class GrpcChannelsProvider : IGrpcChannelsProvider
    {
        private readonly static string _grpcMeasureUrl = Environment.GetEnvironmentVariable("GRPC_MEASURES");
        private readonly static string _grpcMeasureUrl2 = Environment.GetEnvironmentVariable("GRPC_MEASURES2");

        private GrpcChannel _measureChannel = null;
        private GrpcChannel _measureChannel2 = null;

        private readonly GrpcChannelOptions _options = null;
        private readonly object _measureLock = new object();
        private readonly object _measureLock2 = new object();

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

        public GrpcChannel GetMeasureChannel() => CreateChannelAsync(ref _measureChannel, _grpcMeasureUrl, _measureLock);
        public GrpcChannel GetMeasureChannel2() => CreateChannelAsync(ref _measureChannel2, _grpcMeasureUrl2, _measureLock2);

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
