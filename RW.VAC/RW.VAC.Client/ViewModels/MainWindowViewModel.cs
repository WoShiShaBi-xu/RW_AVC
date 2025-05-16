using System.Security.Claims;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RW.Framework.Extensions;
using RW.Framework.Security.Auth;
using RW.VAC.Client.Models;

namespace RW.VAC.Client.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
	private readonly IServiceProvider _serviceProvider;

    private readonly IHost _host;

    [ObservableProperty] private string _applicationTitle = string.Empty;
	
	[ObservableProperty] [NotifyPropertyChangedFor(nameof(HasValue))]

	private string _userName = "操作员";

	private bool _isInitialized;

	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		if (!_isInitialized) InitializeViewModel();
	}

	public bool HasValue => UserName.NotNullOrWhiteSpace();

	public List<DropDownMenuItem> HeadAction { get; } = new();

	private void InitializeViewModel()
	{
		ApplicationTitle = "蝶阀自动装配线中控";
		HeadAction.Add(new DropDownMenuItem("注销", "\xe62a", new RelayCommand(OnQuit)));
		_isInitialized = true;
	}

	[RelayCommand]
    private async void OnQuit( )
    {
        var authService = _serviceProvider.GetRequiredService<ExternalAuthService>();
        if ( authService.CurrentUser != null )
        {
            var id = authService.CurrentUser.FindFirst( ClaimTypes.NameIdentifier )?.Value;
            if ( Guid.TryParse( id , out var userId ) )
            {
                using ( var scope = _serviceProvider.CreateScope() )
                {
                   
                }
            }
        }

        authService.CurrentUser = new ClaimsPrincipal();
        UserName = string.Empty;

        // 可选：关闭应用程序
        // Application.Current.Shutdown();
    }
}