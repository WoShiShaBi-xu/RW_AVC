using Autofac;
using RW.Framework.Auditing;
using RW.Framework.Autofac.Start;
using RW.Framework.Domain.Services;
using RW.Framework.EventBus;
using RW.Framework.EventBus.Local;
using RW.Framework.Guids;
using RW.Framework.Security.Users;

namespace RW.Framework.Autofac.Modules;

public class FrameworkModule: Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<SequentialGuidGenerator>().As<IGuidGenerator>();
		builder.RegisterType<CurrentUser>().As<ICurrentUser>();
		builder.RegisterType<AuditPropertySetter>().As<IAuditPropertySetter>();
		builder.RegisterType<DomainService>().As<IDomainService>().PropertiesAutowired();

		builder.RegisterType<LocalEventBus>().As<IEventBus>().SingleInstance();
		builder.RegisterType<StartForEventBus>().As<IStartable>().SingleInstance();
		
	}
}