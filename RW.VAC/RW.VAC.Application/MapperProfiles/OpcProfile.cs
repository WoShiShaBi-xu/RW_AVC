using AutoMapper;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Domain.Opcs;

namespace RW.VAC.Application.MapperProfiles;

public class OpcProfile : Profile
{
	public OpcProfile()
	{
		CreateMap<OpcGroup, OpcGroupDto>();

		CreateMap<OpcItem, OpcItemDto>();

		CreateMap<OpcGroupDto, OpcGroupCreateUpdateDto>();

		CreateMap<OpcGroupCreateUpdateDto, OpcGroup>();

		CreateMap<OpcItemDto, OpcItemCreateUpdateDto>();
	}
}