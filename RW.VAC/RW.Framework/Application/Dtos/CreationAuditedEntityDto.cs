using RW.Framework.Auditing;

namespace RW.Framework.Application.Dtos;

[Serializable]
public abstract class CreationAuditedEntityDto : EntityDto, ICreationAuditedObject
{
	public DateTime CreationTime { get; set; }

	public Guid? CreatorId { get; set; }
}

[Serializable]
public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAuditedObject
{
	public DateTime CreationTime { get; set; }


	public Guid? CreatorId { get; set; }
}