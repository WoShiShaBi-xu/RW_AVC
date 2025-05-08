namespace RW.Framework.Security.Users;

public interface ICurrentUser
{
	bool IsAuthenticated { get; }

	Guid? Id { get; }

	string? UserName { get; }
}