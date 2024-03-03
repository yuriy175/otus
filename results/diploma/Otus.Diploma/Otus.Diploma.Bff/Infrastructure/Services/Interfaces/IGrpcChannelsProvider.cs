using Grpc.Net.Client;

namespace Bff.Infrastructure.Services.Interfaces
{
    public interface IGrpcChannelsProvider
    {
        GrpcChannel GetMeasureChannel();
        GrpcChannel GetMeasureChannel2();
    }
}
