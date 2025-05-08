using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RW.Framework.Autofac.Modules;
using RW.Framework.Config;
using RW.Framework.Security.Auth;
using RW.VAC.Application.Events.EventData;
using RW.VAC.Dashboard.Bridge;
using RW.VAC.Dashboard.service;
using RW.VAC.Dashboard.settings;
using RW.VAC.Dashboard.subscribe;
using RW.VAC.Domain.Opcs;
using RW.VAC.Domain.Parameters;
using RW.VAC.Domain.Products;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Opc;
using WindowsFormsLifetime;

namespace RW.VAC.Dashboard
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			_ = new Mutex(false, Assembly.GetExecutingAssembly().FullName, out var running);
			if (!running)
			{
				MessageBox.Show("程序已运行，请勿重复启动！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			CreateHostBuilder().Build().Run();
		}

		public static IHostBuilder CreateHostBuilder()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", false, true)
				.Build();

			return Host.CreateDefaultBuilder()
				.ConfigureAppConfiguration(config =>
				{
					config.AddConfiguration(configuration);
				})
				.ConfigureServices((context, services) =>
				{
					services.AddHostedService<ApplicationHostService>();
					services.AddHostedService<CommunicationHostService>();
					

					var applicationLayer = configuration["LayerConfig:ApplicationLayer"];
					if (applicationLayer == null) throw new ConfigurationErrorsException("应用服务层配置错误");
					services.AddAutoMapper(Assembly.Load(applicationLayer));
					services.AddConfig(configuration);
				})
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureContainer<ContainerBuilder>(builder =>
				{
					builder.RegisterModule(new FrameworkModule());
					builder.RegisterModule(new FreeSqlModule(configuration));
					builder.RegisterModule(new RepositoryModule(configuration));
					builder.RegisterModule(new ServiceModule(configuration));

					builder.RegisterType<ExternalAuthService>().SingleInstance();
					builder.RegisterType<ExternalAuthenticationStateProvider>().As<AuthenticationStateProvider>();

					builder.RegisterType<SettingProvider>().SingleInstance();
					builder.RegisterType<TagStorage>().SingleInstance();
					builder.RegisterType<UaClient>().As<IUaClient>().SingleInstance();
					builder.RegisterType<DeviceSubscribe>().SingleInstance();
					
					
					//TODO:领域服务统一注册
					builder.RegisterType<OpcGroupManager>().InstancePerLifetimeScope();
					builder.RegisterType<OpcItemManager>().InstancePerLifetimeScope();
					builder.RegisterType<ProductManager>().InstancePerLifetimeScope();
					builder.RegisterType<ProductionRecordManager>().InstancePerLifetimeScope();
					builder.RegisterType<ParameterManager>().InstancePerLifetimeScope();
					builder.RegisterType<CapacityBridge>().InstancePerLifetimeScope();
				})
			.UseWindowsFormsLifetime<FrmMain>();
		}
	}
}