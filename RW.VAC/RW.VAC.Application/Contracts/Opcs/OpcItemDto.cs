using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Opcs;

public class OpcItemDto : EntityDto<Guid>
{
	public Guid GroupId { get; set; }

	public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsProcess { get; set; }
}