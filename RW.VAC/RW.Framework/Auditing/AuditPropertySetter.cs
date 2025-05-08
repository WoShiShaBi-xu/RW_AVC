using RW.Framework.Core;
using RW.Framework.Security.Users;

namespace RW.Framework.Auditing;

public class AuditPropertySetter(ICurrentUser currentUser) : IAuditPropertySetter
{
	protected ICurrentUser CurrentUser = currentUser;

	public void SetCreationProperties(object targetObject)
	{
		SetCreationTime(targetObject);
		SetCreatorId(targetObject);
	}

	public void SetModificationProperties(object targetObject)
	{
		SetLastModificationTime(targetObject);
		SetLastModifierId(targetObject);
	}

	public void SetDeletionProperties(object targetObject)
	{
		SetDeletionTime(targetObject);
		SetDeleterId(targetObject);
	}

	protected virtual void SetCreationTime(object targetObject)
	{
		if (!(targetObject is IHasCreationTime objectWithCreationTime))
		{
			return;
		}

		if (objectWithCreationTime.CreationTime == default)
		{
			ObjectHelper.TrySetProperty(objectWithCreationTime, x => x.CreationTime, () => DateTime.Now);
		}
	}

	protected virtual void SetCreatorId(object targetObject)
	{
		if (!CurrentUser.Id.HasValue)
		{
			return;
		}

		if (targetObject is not ICreationAuditedObject creationAuditedObject)
		{
			return;
		}

		if (creationAuditedObject.CreatorId.HasValue && creationAuditedObject.CreatorId.Value != default)
		{
			return;
		}

		ObjectHelper.TrySetProperty(creationAuditedObject, x => x.CreatorId, () => CurrentUser.Id);
	}

	protected virtual void SetLastModificationTime(object targetObject)
	{
		if (targetObject is IHasModificationTime objectWithModificationTime)
		{
			ObjectHelper.TrySetProperty(objectWithModificationTime, x => x.LastModificationTime, () => DateTime.Now);
		}
	}

	protected virtual void SetLastModifierId(object targetObject)
	{
		if (targetObject is not IModificationAuditedObject modificationAuditedObject)
		{
			return;
		}

		if (!CurrentUser.Id.HasValue)
		{
			ObjectHelper.TrySetProperty(modificationAuditedObject, x => x.LastModifierId, () => null);
			return;
		}

		ObjectHelper.TrySetProperty(modificationAuditedObject, x => x.LastModifierId, () => CurrentUser.Id);
	}

	protected virtual void SetDeletionTime(object targetObject)
	{
		if (targetObject is IHasDeletionTime {DeletionTime: null} objectWithDeletionTime)
		{
			ObjectHelper.TrySetProperty(objectWithDeletionTime, x => x.DeletionTime, () => DateTime.Now);
		}
	}

	protected virtual void SetDeleterId(object targetObject)
	{
		if (targetObject is not IDeletionAuditedObject deletionAuditedObject)
		{
			return;
		}

		if (deletionAuditedObject.DeleterId != null)
		{
			return;
		}

		if (!CurrentUser.Id.HasValue)
		{
			ObjectHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => null);
			return;
		}

		ObjectHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => CurrentUser.Id);
	}
}