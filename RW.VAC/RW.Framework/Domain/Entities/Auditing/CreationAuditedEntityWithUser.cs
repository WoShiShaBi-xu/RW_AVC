using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class CreationAuditedEntityWithUser<TUser> : CreationAuditedEntity, ICreationAuditedObject<TUser>
{
    public virtual TUser? Creator { get; protected set; }
}

[Serializable]
public abstract class CreationAuditedEntityWithUser<TKey, TUser> : CreationAuditedEntity<TKey>,
    ICreationAuditedObject<TUser>
{
    protected CreationAuditedEntityWithUser()
    {
    }

    protected CreationAuditedEntityWithUser(TKey id) : base(id)
    {
    }

    public virtual TUser? Creator { get; protected set; }
}