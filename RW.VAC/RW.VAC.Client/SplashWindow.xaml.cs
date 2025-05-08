using System.Windows;
using RW.VAC.Client.ViewModels;

namespace RW.VAC.Client;

/// <summary>
///     SplashWindow.xaml 的交互逻辑
/// </summary>
public partial class SplashWindow : Window
{
	public SplashWindow(SplashWindowViewModel viewModel)
	{
		ViewModel = viewModel;
		DataContext = this;
		InitializeComponent();
	}

	public SplashWindowViewModel ViewModel { get; }
}