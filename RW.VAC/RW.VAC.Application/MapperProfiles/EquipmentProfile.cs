using AutoMapper;
using RW.VAC.Application.Contracts.Equipments;
using RW.VAC.Domain.Equipments;

namespace RW.VAC.Application.MapperProfiles;

public class EquipmentProfile : Profile
{
	public EquipmentProfile()
	{
		CreateMap<Equipment, EquipmentDto>();
	}
}