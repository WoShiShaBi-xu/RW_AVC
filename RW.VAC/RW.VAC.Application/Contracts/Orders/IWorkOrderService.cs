using RW.Framework.Application.Services;
using RW.VAC.Domain.Orders;

namespace RW.VAC.Application.Contracts.Orders;

public interface IWorkOrderService : ICrudAppService<WorkOrder, Guid, WorkOrderDto, WorkOrderPagedListRequestDto,
	WorkOrderCreateUpdateDto, WorkOrderCreateUpdateDto>
{
}