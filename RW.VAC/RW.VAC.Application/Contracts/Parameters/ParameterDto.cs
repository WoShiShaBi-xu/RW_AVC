using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Parameters;

public class ParameterDto : EntityDto<Guid>
{
	public string Code { get; set; }

	public string Value { get; set; }

	public string Description { get; set; }
}