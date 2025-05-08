using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class FullAuditedEntity : AuditedEntity, IFullAuditedObject
{
    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }

    public virtual bool IsDeleted { get; set; }
}

[Serializable]
public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }

    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }

    protected FullAuditedEntity()
    {
    }

    protected FullAuditedEntity(TKey id) : base(id)
    {
    }
}