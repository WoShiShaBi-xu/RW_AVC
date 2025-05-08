using RW.Framework.Auditing;

namespace RW.Framework.Application.Dtos;

[Serializable]
public abstract class FullAuditedEntityDto : AuditedEntityDto, IFullAuditedObject
{
	public bool IsDeleted { get; set; }

	public Guid? DeleterId { get; set; }

	public DateTime? DeletionTime { get; set; }
}

[Serializable]
public abstract class FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IFullAuditedObject
{
	public bool IsDeleted { get; set; }

	public Guid? DeleterId { get; set; }

	public DateTime? DeletionTime { get; set; }
}