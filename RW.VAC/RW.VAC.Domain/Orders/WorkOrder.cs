using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;

namespace RW.VAC.Domain.Orders;

[Table(Name = "Work_Order")]
public class WorkOrder : AggregateRoot<Guid>
{
	public WorkOrder(string orderNo, Guid productId, int quantity, DateTime plannedStartTime)
	{
		OrderNo = orderNo;
		ProductId = productId;
		Quantity = quantity;
		PlannedStartTime = plannedStartTime;
	}

	public WorkOrder(Guid id, string orderNo, Guid productId, int quantity, DateTime plannedStartTime) : base(id)
	{
		OrderNo = orderNo;
		ProductId = productId;
		Quantity = quantity;
		PlannedStartTime = plannedStartTime;
	}

	[Column(StringLength = 100)]
	public string OrderNo { get; set; }

	public Guid ProductId { get; set; }

	public int Quantity { get; set; }

	public DateTime PlannedStartTime { get; set; }

	public DateTime? PlannedEndTime { get; set; }

	public DateTime? ActualStartTime { get; set; }

	public DateTime? ActualEndTime { get; set; }

	public OrderStatus Status { get; set; }
}