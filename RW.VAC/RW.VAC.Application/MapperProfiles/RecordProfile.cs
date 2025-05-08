using AutoMapper;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.MapperProfiles;

public class RecordProfile : Profile
{
	public RecordProfile()
	{
		CreateMap<ProductionRecord, ProductionRecordDto>();

		CreateMap<ProductionDetail, ProductionDetailDto>();

		CreateMap<ProductionData, ProductionDataDto>();
	}
}