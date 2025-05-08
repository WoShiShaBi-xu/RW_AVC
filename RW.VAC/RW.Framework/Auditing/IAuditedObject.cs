namespace RW.Framework.Auditing;

public interface IAuditedObject : ICreationAuditedObject, IModificationAuditedObject
{
}

public interface IAuditedObject<TUser> : IAuditedObject, ICreationAuditedObject<TUser>,
    IModificationAuditedObject<TUser>
{
}