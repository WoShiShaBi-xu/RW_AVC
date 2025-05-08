using RW.Framework.Auditing;

namespace RW.Framework.Application.Dtos;

[Serializable]
public abstract class CreationAuditedEntityWithUserDto<TUserDto> : CreationAuditedEntityDto, ICreationAuditedObject<TUserDto>
{
	public TUserDto? Creator { get; set; }
}

[Serializable]
public abstract class CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : CreationAuditedEntityDto<TPrimaryKey>, ICreationAuditedObject<TUserDto>
{
	public TUserDto? Creator { get; set; }
}