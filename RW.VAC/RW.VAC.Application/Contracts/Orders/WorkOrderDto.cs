using RW.Framework.Application.Dtos;
using RW.VAC.Domain.Orders;

namespace RW.VAC.Application.Contracts.Orders;

public class WorkOrderDto : EntityDto<Guid>
{
	public string OrderNo { get; set; }

	public Guid ProductId { get; set; }

	public string ProductName { get; set; }

	public string ProductCode { get; set; }

	public int Quantity { get; set; }

	public DateTime PlannedStartTime { get; set; }

	public DateTime? PlannedEndTime { get; set; }

	public OrderStatus Status { get; set; }
}