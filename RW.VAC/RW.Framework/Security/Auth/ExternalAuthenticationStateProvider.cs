
using Microsoft.AspNetCore.Components.Authorization;
using RW.Framework.Security.Users;
using System.Security.Claims;

namespace RW.Framework.Security.Auth;

public class ExternalAuthenticationStateProvider : AuthenticationStateProvider
{
	private AuthenticationState _currentUser;
	
	public ExternalAuthenticationStateProvider(ExternalAuthService service)
	{
		_currentUser = new AuthenticationState(service.CurrentUser);
		service.UserChanged += user =>
		{
			_currentUser = new AuthenticationState(user);
			NotifyAuthenticationStateChanged(Task.FromResult(_currentUser));
		};
	}
	
	public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(_currentUser);
}