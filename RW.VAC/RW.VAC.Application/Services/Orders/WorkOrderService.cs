using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.VAC.Application.Contracts.Orders;
using RW.VAC.Domain.Orders;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Services.Orders;

public class WorkOrderService(
	IWorkOrderRepository repository)
	: CrudAppService<WorkOrder, Guid, WorkOrderDto, WorkOrderPagedListRequestDto,
		WorkOrderCreateUpdateDto, WorkOrderCreateUpdateDto>(repository), IWorkOrderService
{
	public override async Task<PagedResultDto<WorkOrderDto>> GetPagedListAsync(WorkOrderPagedListRequestDto input)
	{
		var list = await Repository.Select.From<Product>((o, p) => o.LeftJoin(op => op.ProductId == p.Id))
			.OrderByDescending(t => t.t1.PlannedStartTime)
			.Count(out var total).Page(input.PageIndex, input.Count).ToListAsync((o, p) => new WorkOrderDto
			{
				Id = o.Id,
				ProductId = o.ProductId,
				ProductName = p.Name,
				ProductCode = p.Code,
				Quantity = o.Quantity,
				PlannedStartTime = o.PlannedStartTime,
				PlannedEndTime = o.PlannedEndTime,
				Status = o.Status
			});
		return new PagedResultDto<WorkOrderDto>(total, list);
	}
}