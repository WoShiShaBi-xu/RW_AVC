using System.ComponentModel;

namespace RW.VAC.Domain.Users;

public enum RoleTypes : byte
{
	[Description("管理员")]
	Admin = 1,

	[Description("用户")]
	User = 2,

    [Description( "生产人员" )]
    ProductionStaff = 3

}