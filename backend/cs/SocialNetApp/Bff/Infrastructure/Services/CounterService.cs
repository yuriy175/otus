using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
using CounterGrpc;
using DialogGrpc;
using static CounterGrpc.Counter;

namespace Bff.Infrastructure.gRpc.Services
{
    public class CounterService : ICounterService
    {
        private readonly IMapper _mapper;
        private readonly IGrpcChannelsProvider _channelsProvider;

        public CounterService(IMapper mapper, IGrpcChannelsProvider channelsProvider)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
        }

        public async Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken)
        {
            var counterClient = new CounterClient(_channelsProvider.GetCountsChannel());
            var reply = await counterClient.GetUnreadCountAsync(new GetUnreadCountRequest { UserId = userId }, cancellationToken: cancellationToken);

            return reply.Count;
        }
    }
}
