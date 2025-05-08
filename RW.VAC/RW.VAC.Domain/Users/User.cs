using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities.Auditing;
using RW.Framework.Extensions;

namespace RW.VAC.Domain.Users;

[Table(Name = "User")]
public class User : FullAuditedAggregateRoot<Guid>
{
	public User(string userName, string account, string password, RoleTypes role, bool isSystemUser)
	{
		UserName = userName;
		Account = account;
		Password = password;
		Role = role;
		IsSystemUser = isSystemUser;

    }

	public User(Guid id, string userName, string account, string password, RoleTypes role, bool isSystemUser) : base(id)
	{
		UserName = userName;
		Account = account;
		Password = password;
		Role = role;
		IsSystemUser = isSystemUser;
    }

	[Column(StringLength = 20)]
	public string UserName { get; set; }

	[Column(StringLength = 20)]
	public string Account { get; set; }

	[Column(StringLength = 50)]
	public string Password { get; set; }

	public RoleTypes Role { get; set; }

	[Column(CanUpdate = false)]
	public bool IsSystemUser { get; set; }

    public bool IsLoggedIn { get; set; }

    public void Update(string userName, string account, string? password,RoleTypes role)
	{
		this.UserName = userName;
		this.Account = account;
		this.Role = role;
		if (password.NotNullOrWhiteSpace()) this.Password = password!;
	}
}