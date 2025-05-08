using System.Configuration;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace RW.Framework.Autofac.Modules;

public class RepositoryModule(IConfiguration configuration) : Module
{
	public IConfiguration Configuration { get; } = configuration;

	protected override void Load(ContainerBuilder builder)
	{
		var infrastructureLayer = Configuration["LayerConfig:InfrastructureLayer"];
		if (infrastructureLayer == null) throw new ConfigurationErrorsException("基础设施层配置错误");
		var assembly = System.Reflection.Assembly.Load(infrastructureLayer);
		builder.RegisterAssemblyTypes(assembly)
			.Where(t => t.Name.EndsWith("Repository"))
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();
	}
}