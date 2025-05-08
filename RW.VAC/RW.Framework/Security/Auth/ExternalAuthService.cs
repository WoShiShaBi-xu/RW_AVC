using System.Security.Claims;

namespace RW.Framework.Security.Auth;

public class ExternalAuthService
{
	public event Action<ClaimsPrincipal>? UserChanged;
	private ClaimsPrincipal? _currentUser;

	public ClaimsPrincipal CurrentUser
	{
		//get => _currentUser ??= new ClaimsPrincipal();
		get => _currentUser ??= new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, "3a112fa5-6bc4-ed34-e4e8-5b2b4dc9fe5f"),
			new(ClaimTypes.Name, "操作员"),
			new(ClaimTypes.Role, "User")
		}, "UserAuth"));
		set
		{
			_currentUser = value;
			UserChanged?.Invoke(_currentUser);
		}
	}
}