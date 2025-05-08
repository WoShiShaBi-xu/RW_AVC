using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class CreationAuditedEntity : Entity, ICreationAuditedObject
{
    public virtual DateTime CreationTime { get; protected set; }

    public virtual Guid? CreatorId { get; protected set; }
}

[Serializable]
public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedObject
{
    protected CreationAuditedEntity()
    {
    }

    protected CreationAuditedEntity(TKey id) : base(id)
    {
    }

    public virtual DateTime CreationTime { get; protected set; }

    public virtual Guid? CreatorId { get; protected set; }
}