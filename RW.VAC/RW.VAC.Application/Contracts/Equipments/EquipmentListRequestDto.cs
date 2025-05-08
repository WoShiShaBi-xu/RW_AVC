using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Equipments;

public class EquipmentListRequestDto : ISortedResultRequest
{
	public bool? IsAutomatic { get; set; }

	public List<(string, bool)> Sorting { get; set; }
}