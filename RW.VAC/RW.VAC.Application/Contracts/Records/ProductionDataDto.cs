using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Records;

public class ProductionDataDto : EntityDto<Guid>
{
	public string Name { get; set; }

	public string Value { get; set; }

	public string? Unit { get; set; }
}