using AutoMapper;
using CounterGrpc;
using Counters.Core.Model.Interfaces;
using Grpc.Core;
using static CounterGrpc.Counter;

namespace Counters.Infrastructure.gRpc.Services
{
    public class CounterService : CounterBase
    {
        private readonly IMapper _mapper;
        private readonly ICounterService _counterService;

        public CounterService(IMapper mapper, ICounterService counterService)
        {
            _mapper = mapper;
            _counterService = counterService;
        }

        public override async Task<GetUnreadCountReply> GetUnreadCount(GetUnreadCountRequest request, ServerCallContext context)
        {
            var count = await _counterService.GetUnReadCounterByUserIdAsync(request.UserId, context.CancellationToken);

            return new GetUnreadCountReply { Count = count};
        }
    }
}
