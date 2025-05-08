using RW.Framework.Auditing;

namespace RW.Framework.Application.Dtos;

[Serializable]
public abstract class AuditedEntityDto : CreationAuditedEntityDto, IAuditedObject
{
	public DateTime? LastModificationTime { get; set; }

	public Guid? LastModifierId { get; set; }
}

[Serializable]
public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IAuditedObject
{
	public DateTime? LastModificationTime { get; set; }

	public Guid? LastModifierId { get; set; }
}