namespace RW.Framework.Auditing;

public interface IFullAuditedObject : IAuditedObject, IDeletionAuditedObject
{
}

public interface IFullAuditedObject<TUser> : IFullAuditedObject, IAuditedObject<TUser>, IDeletionAuditedObject<TUser>
{
}