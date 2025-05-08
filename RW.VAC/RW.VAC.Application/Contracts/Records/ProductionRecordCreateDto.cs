using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Contracts.Records;

public class ProductionRecordCreateDto(
	string serialNumber,
	string productName,
	string productCode,
	RoutingTypes routing,
	List<string> process)
{
	public string SerialNumber { get; set; } = serialNumber;

	public string ProductName { get; set; } = productName;

	public string ProductCode { get; set; } = productCode;

	public RoutingTypes Routing { get; set; } = routing;

	public List<string> Process { get; set; } = process;
}