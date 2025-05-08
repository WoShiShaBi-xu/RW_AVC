using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using RW.Framework.Extensions;

namespace RW.Framework.Security.Users;

public class CurrentUser(AuthenticationStateProvider authenticationStateProvider) : ICurrentUser
{
	private readonly AuthenticationState _authenticationState = authenticationStateProvider.GetAuthenticationStateAsync().Result;

	public bool IsAuthenticated => _authenticationState.User.Identity is {IsAuthenticated: true};

	public Guid? Id
	{
		get
		{
			var id = _authenticationState.User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
			if (!id.NotNullOrWhiteSpace()) return Guid.Empty;
			return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
		}
	}

	public string? UserName => _authenticationState.User.Identity?.Name;
}