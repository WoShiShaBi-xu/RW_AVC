using RW.Framework.Auditing;

namespace RW.Framework.Application.Dtos;

[Serializable]
public abstract class FullAuditedEntityWithUserDto<TUserDto> : FullAuditedEntityDto, IFullAuditedObject<TUserDto>
{
	public TUserDto? Creator { get; set; }

	public TUserDto? LastModifier { get; set; }

	public TUserDto? Deleter { get; set; }
}

[Serializable]
public abstract class FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : FullAuditedEntityDto<TPrimaryKey>, IFullAuditedObject<TUserDto>
{
	public TUserDto? Creator { get; set; }

	public TUserDto? LastModifier { get; set; }

	public TUserDto? Deleter { get; set; }
}