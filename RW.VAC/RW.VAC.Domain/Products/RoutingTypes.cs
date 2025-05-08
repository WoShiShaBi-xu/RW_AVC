using System.ComponentModel;

namespace RW.VAC.Domain.Products;

public enum RoutingTypes
{
	/// <summary>
	///		硬密封阀
	/// </summary>
	[Description("硬密封阀")]
	Seal,

	/// <summary>
	///		真空阀
	/// </summary>
	[Description("真空阀")]
	Vacuum
}