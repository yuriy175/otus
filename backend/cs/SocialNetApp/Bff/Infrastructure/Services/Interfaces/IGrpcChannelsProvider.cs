using Grpc.Net.Client;

namespace Bff.Infrastructure.Services.Interfaces
{
    public interface IGrpcChannelsProvider
    {
        GrpcChannel GetAuthChannel();
        GrpcChannel GetUsersChannel();
        GrpcChannel GetDialogsChannel();
        GrpcChannel GetPostsChannel();
        GrpcChannel GetCountsChannel();
    }
}
