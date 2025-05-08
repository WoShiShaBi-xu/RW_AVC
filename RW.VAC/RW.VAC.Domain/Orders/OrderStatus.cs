using System.ComponentModel;

namespace RW.VAC.Domain.Orders;

public enum OrderStatus
{
	[Description("待生产")]
	ToBeExecuted,

	[Description("生产中")]
	Executing,

	[Description("完成")]
	Completed
}