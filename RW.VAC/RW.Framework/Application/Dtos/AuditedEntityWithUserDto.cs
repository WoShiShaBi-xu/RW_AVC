using RW.Framework.Auditing;

namespace RW.Framework.Application.Dtos;

[Serializable]
public abstract class AuditedEntityWithUserDto<TUserDto> : AuditedEntityDto, IAuditedObject<TUserDto>
{
	public TUserDto? Creator { get; set; }

	public TUserDto? LastModifier { get; set; }
}

[Serializable]
public abstract class AuditedEntityWithUserDto<TPrimaryKey, TUserDto> : AuditedEntityDto<TPrimaryKey>, IAuditedObject<TUserDto>
{
	public TUserDto? Creator { get; set; }

	public TUserDto? LastModifier { get; set; }
}