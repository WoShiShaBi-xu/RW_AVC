﻿namespace RW.Framework.Auditing;

public interface IAuditPropertySetter
{
	void SetCreationProperties(object targetObject);

	void SetModificationProperties(object targetObject);

	void SetDeletionProperties(object targetObject);
}