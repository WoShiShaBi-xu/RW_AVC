using AutoMapper;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Domain.Parameters;

namespace RW.VAC.Application.MapperProfiles;

public class ParameterProfile : Profile
{
	public ParameterProfile()
	{
		CreateMap<Parameter, ParameterDto>();

		CreateMap<ParameterDto, ParameterCreateUpdateDto>();
	}
}