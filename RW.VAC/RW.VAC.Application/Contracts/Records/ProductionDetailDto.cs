using RW.Framework.Application.Dtos;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Contracts.Records;

public class ProductionDetailDto : EntityDto<Guid>
{
	public string ProcessName { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public ProcessStatus Status { get; set; }

	public IList<ProductionDataDto>? Data { get; set; }
}