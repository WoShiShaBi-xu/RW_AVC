using Autofac;
using Microsoft.Extensions.Configuration;
using RW.Framework.Application.Services;
using RW.Framework.EventBus;
using System.Configuration;

namespace RW.Framework.Autofac.Modules;

public class ServiceModule(IConfiguration configuration) : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<ApplicationService>().As<IApplicationService>().InstancePerLifetimeScope()
			.PropertiesAutowired();
		var applicationLayer = configuration["LayerConfig:ApplicationLayer"];
		if (applicationLayer == null) throw new ConfigurationErrorsException("应用服务层配置错误");
		var assembly = System.Reflection.Assembly.Load(applicationLayer);
		builder.RegisterAssemblyTypes(assembly)
			.Where(t => t.Name.EndsWith("Service"))
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();

		builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>)).InstancePerDependency();
	}
}