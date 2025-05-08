using Microsoft.AspNetCore.Components;

namespace RW.VAC.Client;

public class NavigationProvider
{
	private NavigationManager? _navigationManager;

	public void Initialize(NavigationManager navigationManager)
	{
		_navigationManager = navigationManager;
	}

	public void NavigateTo(string url)
	{
		_navigationManager?.NavigateTo(url);
	}
}