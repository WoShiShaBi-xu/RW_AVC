using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RW.VAC.Client.Services;

public class ApplicationHostService(IServiceProvider serviceProvider) : IHostedService
{
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await HandleActivationAsync();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	private Task HandleActivationAsync()
	{
		if (!System.Windows.Application.Current.Windows.OfType<SplashWindow>().Any())
		{
			var splashWindow = serviceProvider.GetRequiredService<SplashWindow>();
			splashWindow.Show();
		}

		return Task.CompletedTask;
	}
}