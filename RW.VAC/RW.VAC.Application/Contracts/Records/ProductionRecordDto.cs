using RW.Framework.Application.Dtos;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Contracts.Records;

public class ProductionRecordDto : EntityDto<Guid>
{
	public string SerialNumber { get; set; }

	public string ProductName { get; set; }

	public string ProductCode { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public ProdStatus Status { get; set; }
}