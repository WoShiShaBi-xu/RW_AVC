using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Equipments;

public class EquipmentDto : EntityDto<Guid>
{
	public string Name { get; set; }

	public string Code { get; set; }

	public bool IsAutomatic { get; set; }
}