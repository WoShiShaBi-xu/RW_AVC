using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Opcs;

public class OpcGroupDto : EntityDto<Guid>
{
	public string Name { get; set; }

	public string Code { get; set; }

	public string Device { get; set; }

	public string Group { get; set; }

}