using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using RW.VAC.Application.Hardwares.Opc;
using RW.VAC.Client.ViewModels;

namespace RW.VAC.Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
	public MainWindowViewModel ViewModel { get; }

	private readonly IServiceProvider _serviceProvider;

	private readonly System.Timers.Timer _timer = new();

	public MainWindow(IServiceProvider serviceProvider, MainWindowViewModel viewModel, GeneralControl generalControl)
	{
		_serviceProvider = serviceProvider;
		ViewModel = viewModel;
		DataContext = this;
		InitializeComponent();
		Resources.Add("services", serviceProvider);
		MainWebView.BlazorWebViewInitialized += (_, _) =>
		{
#if !DEBUG
			MainWebView.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
			MainWebView.WebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
			MainWebView.WebView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
			MainWebView.WebView.CoreWebView2.Settings.IsPinchZoomEnabled = false;
			MainWebView.WebView.CoreWebView2.Settings.IsZoomControlEnabled = false;
			MainWebView.WebView.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
#endif
			MainWebView.WebView.NavigationCompleted += WebView_NavigationCompleted;
		};
	}

	private async void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		var vm = _serviceProvider.GetRequiredService<SplashWindowViewModel>();
		vm.Progress = 100;
		await Task.Delay(500);
		var splashWindow = _serviceProvider.GetRequiredService<SplashWindow>();
		splashWindow.Close();
		ShowInTaskbar = true;
		WindowState = WindowState.Maximized;
		SetWindow();
		//SetHeartbeat();
	}

	private void SetWindow()
	{
		this.Width = 800;
		this.Height = 450;
		var screenWidth = SystemParameters.PrimaryScreenWidth;
		var screenHeight = SystemParameters.PrimaryScreenHeight;
		this.Left = (screenWidth / 2) - (this.Width / 2);
		this.Top = (screenHeight / 2) - (this.Height / 2);
	}

	private void SetHeartbeat()
	{
		var generalControl = _serviceProvider.GetRequiredService<GeneralControl>();
		var global = _serviceProvider.GetRequiredService<Global>();
		double interval = double.TryParse(global.Parameter["Heartbeat"], out interval) ? interval : 200;
		_timer.Interval = interval;
		_timer.Elapsed += (_, _) =>
		{
			generalControl.Heartbeat();
		};
		_timer.Start();
	}
}