using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Contracts.Records;

public class ProdRecordQueryDto
{
	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public ProdStatus Status { get; set; }
}