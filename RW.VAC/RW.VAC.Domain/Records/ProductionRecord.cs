using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;
using RW.VAC.Domain.Products;

namespace RW.VAC.Domain.Records;

[Table(Name = "Production_Record")]
public class ProductionRecord : AggregateRoot<Guid>
{
	public ProductionRecord(string serialNumber, string productName, string productCode, RoutingTypes routing,
		DateTime startTime)
	{
		SerialNumber = serialNumber;
		ProductName = productName;
		ProductCode = productCode;
		Routing = routing;
		StartTime = startTime;
	}

	public ProductionRecord(Guid id, string serialNumber, string productName, string productCode, RoutingTypes routing,
		DateTime startTime) : base(id)
	{
		SerialNumber = serialNumber;
		ProductName = productName;
		ProductCode = productCode;
		Routing = routing;
		StartTime = startTime;
	}

	[Column(StringLength = 50)] public string SerialNumber { get; set; }

	[Column(StringLength = 50)] public string ProductName { get; set; }

	[Column(StringLength = 50)] public string ProductCode { get; set; }

	public RoutingTypes Routing { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public ProdStatus Status { get; set; }

	public int OfflineCount { get; set; }

	public void SetAbnormalOffline()
	{
		if (Status == ProdStatus.InProgress) Status = ProdStatus.AbnormalOffline;
	}

	public void SetBackOnline()
	{
		if (Status == ProdStatus.AbnormalOffline) Status = ProdStatus.BackOnline;
	}

	public void SetCompleted()
	{
		if (Status == ProdStatus.Completed) return;
		Status = ProdStatus.Completed;
        EndTime= DateTime.Now;

    }
}