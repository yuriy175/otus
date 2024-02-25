using Bff.API.Dtos;
using MeasureGrpc;
using Model = Measure.Core.Model;

namespace Bff.API.Mappings
{
    public class MeasureMappings : AutoMapper.Profile
    {
        public MeasureMappings()
        {
            CreateMap<MeasureReply, MeasureDto>().ForMember(m => m.Date, m => m.MapFrom(e => e.Date.ToDateTime()));
            CreateMap<Model.Measure, MeasureDto>();
        }
    }
}