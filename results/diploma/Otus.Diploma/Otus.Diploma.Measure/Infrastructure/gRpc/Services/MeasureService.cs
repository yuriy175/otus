using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Measure.Core.Model.Interfaces;
using MeasureGrpc;
using static MeasureGrpc.Measure;

namespace Measure.Infrastructure.gRpc.Services
{
    public class MeasureService : MeasureBase
    {
        private readonly IMapper _mapper;
        private readonly IMeasureService _measureService;

        public MeasureService(IMapper mapper, IMeasureService measureService)
        {
            _mapper = mapper;
            _measureService = measureService;
        }

        public override async Task<GetMeasuresReply> GetMeasures(GetMeasuresRequest request, ServerCallContext context)
        {
            var measures = await _measureService.GetMeasuresAsync(request.DeviceId, request.UseReplica, context.CancellationToken);
            var reply = new GetMeasuresReply { };

            reply.Measures.AddRange(measures.Select(m => new MeasureReply
            {
                Id = m.Id,
                DeviceId = m.DeviceId,
                Type = m.Type,
                Value = m.Value,
                Date = Timestamp.FromDateTime(DateTime.SpecifyKind(m.Date, DateTimeKind.Utc)),
            }));
            return reply;
        }
    }
}
