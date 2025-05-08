using RW.Framework.Application.Services;
using RW.VAC.Domain.Equipments;

namespace RW.VAC.Application.Contracts.Equipments;

public interface IEquipmentService : IReadOnlyAppService<Equipment, Guid, EquipmentDto, EquipmentListRequestDto>
{
}