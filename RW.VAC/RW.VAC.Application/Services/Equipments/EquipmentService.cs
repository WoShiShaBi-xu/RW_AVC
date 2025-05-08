using System.Linq.Expressions;
using RW.Framework.Application.Services;
using RW.VAC.Application.Contracts.Equipments;
using RW.VAC.Domain.Equipments;

namespace RW.VAC.Application.Services.Equipments;

public class EquipmentService(
	IEquipmentRepository repository)
	: ReadOnlyAppService<Equipment, Guid, EquipmentDto, EquipmentListRequestDto>(repository),
		IEquipmentService
{
	protected override Expression<Func<Equipment, bool>>? GreateFilter(EquipmentListRequestDto input)
	{
		Expression<Func<Equipment, bool>>? where = null;
		where = where.And(input.IsAutomatic.HasValue, t => t.IsAutomatic == input.IsAutomatic);
		return where;
	}
}