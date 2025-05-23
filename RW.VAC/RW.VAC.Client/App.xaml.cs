﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RW.Framework.Autofac;
using RW.Framework.Autofac.Modules;
using RW.Framework.Config;
using RW.Framework.EventBus;
using RW.Framework.Security.Auth;
using RW.VAC.Client.LogSink;
using RW.VAC.Client.Services;
using RW.VAC.Client.Storage;
using RW.VAC.Client.ViewModels;
using RW.VAC.Domain.CodeReaders;
using RW.VAC.Domain.Opcs;
using RW.VAC.Infrastructure.Devices.State;
using RW.VAC.Infrastructure.Opc;
using RW.VAC.Infrastructure.Tcp;
using Serilog;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Client.Interceptors;
using RW.VAC.Application.Contracts.API;
using RW.VAC.Domain.API;
using System;
using System.Security.Claims;
using RW.VAC.Application.Services.DockingPositionService;
using RW.VAC.Application.Services.Locations;
using RW.VAC.Application.Services.Pallets;
using RW.VAC.Application.Services.TasksDo;
using RW.VAC.Domain.DockingPosition;
using RW.VAC.Domain.Location;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.TestStation;
using RW.VAC.Infrastructure.Repositories;
using RW.VAC.Domain.Products;

[assembly: ComponentException]
namespace RW.VAC.Client;

public partial class App
{
	private readonly IHost _host;
	private Mutex _mutex = null!;
    private readonly IServiceProvider _serviceProvider;
    public App()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", false, true)
			.Build();

		_host = Host
			.CreateDefaultBuilder()
			.ConfigureAppConfiguration(config => { config.AddConfiguration(configuration); })
			.ConfigureServices((context, services) =>
			{
				services.AddHostedService<ApplicationHostService>();

				var applicationLayer = configuration["LayerConfig:ApplicationLayer"];
				if (applicationLayer != null)
				{
					services.AddAutoMapper(Assembly.Load(applicationLayer));
					services.AddFluxor(opt => opt.ScanAssemblies(typeof(App).Assembly));
					services.AddWpfBlazorWebView();
#if DEBUG
					services.AddBlazorWebViewDeveloperTools();
#endif
					services.AddAntDesign();
					services.AddConfig(configuration);

					services.AddAuthorizationCore();
				}
				else
				{
					throw new ConfigurationErrorsException("应用服务层配置错误");
				}
                services.AddHttpClient();

            } )
			.UseSerilog((context, services, config) => config.ReadFrom.Configuration(context.Configuration).WriteTo
				.AppClientSink(services.GetRequiredService<IEventBus>()))
			.UseServiceProviderFactory(new AutofacServiceProviderFactory())
			.ConfigureContainer<ContainerBuilder>(builder =>
			{
				builder.RegisterModule(new FrameworkModule());
				builder.RegisterModule(new FreeSqlModule(configuration));
				builder.RegisterModule(new RepositoryModule(configuration));
				builder.RegisterModule(new ServiceModule(configuration));

				builder.RegisterType<ExternalAuthService>().SingleInstance();
				builder.RegisterType<ExternalAuthenticationStateProvider>().As<AuthenticationStateProvider>();

				builder.RegisterType<Global>().SingleInstance();
				builder.RegisterType<LogBuffer>().SingleInstance();
				builder.RegisterType<NavigationProvider>().SingleInstance();
                // 在 ConfigureContainer 方法中添加以下注册
                builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
                builder.RegisterType<DockingPositionService>().As<IDockingPositionService>().SingleInstance();
                builder.RegisterType<PalletService>().As<IPalletService>().SingleInstance();
                builder.RegisterType<TestStationService>().As<ITestStationService>().SingleInstance();
                builder.RegisterType<ProductRepository>().As<IProductRepository>().SingleInstance();
                // 确保仓储也已注册
                builder.RegisterType<LocationRepository>().As<ILocationRepository>().SingleInstance();
                builder.RegisterType<DockingPositionRepository>().As<IDockingPositionRepository>().SingleInstance();
                builder.RegisterType<PalletRepository>().As<IPalletRepository>().SingleInstance();
                builder.RegisterType<TestStationRepository>().As<ITestStationRepository>().SingleInstance();

                builder.RegisterType<SplashWindowViewModel>().SingleInstance();
				builder.RegisterType<SplashWindow>().SingleInstance();

				builder.RegisterType<MainWindowViewModel>().SingleInstance();
				builder.RegisterType<MainWindow>().SingleInstance();

				//TODO:领域服务统一注册
				builder.RegisterType<OpcGroupManager>().InstancePerLifetimeScope();
				builder.RegisterType<OpcItemManager>().InstancePerLifetimeScope();

				#region 读码器相关

				builder.RegisterType<TcpServer>().As<ITcpServer>().SingleInstance();
				builder.RegisterType<CodeReaderState>().SingleInstance();
				builder.RegisterType<CodeQueue>().SingleInstance();
				

				#endregion

				#region OPC相关

				builder.RegisterType<TagStorage>().SingleInstance();
				builder.RegisterType<UaClient>().As<IUaClient>().SingleInstance();
                builder.RegisterType<PLCState>().SingleInstance();


                #endregion
                #region API服务注册
                builder.RegisterType<WMSClient>().As<IAutoAssemblyWorkClient>().SingleInstance();
                #endregion
            } ).Build();
	}


	private void OnStartup(object sender, StartupEventArgs e)
	{
		_mutex = new Mutex(true, Assembly.GetExecutingAssembly().FullName, out var running);
		if (!running)
		{
			MessageBox.Show("程序已运行!", "警告", MessageBoxButton.OK, MessageBoxImage.Stop);
			Shutdown();
		}

		_host.Start();
		IocManager.Instance.Initialize(_host.Services.GetAutofacRoot());
	}

	private async void OnExit(object sender, ExitEventArgs e)
    {
		
		
            


		await _host.StopAsync();
		_host.Dispose();
	}


	private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
	}
}