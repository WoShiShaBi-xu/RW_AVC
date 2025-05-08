using Autofac;
using Microsoft.Extensions.DependencyInjection;
using RW.Framework.EventBus;

namespace RW.Framework.Autofac.Start;

public class StartForEventBus(
	ILifetimeScope lifetimeScope,
	IEventBus eventBus) : IStartable
{
	public void Start()
	{
		var handlers = lifetimeScope.ComponentRegistry.Registrations
			.Where(t => typeof(IEventHandler).IsAssignableFrom(t.Activator.LimitType))
			.Select(t => t.Activator.LimitType).ToList();
		if (!handlers.Any()) return;
		var scopeFactory = lifetimeScope.Resolve<IServiceScopeFactory>();
		foreach (var handler in handlers)
		{
			var interfaces = handler.GetInterfaces();
			foreach (var @interface in interfaces)
			{
				if (!typeof(IEventHandler).IsAssignableFrom(@interface)) continue;
				var genericArgs = @interface.GetGenericArguments();
				if (genericArgs.Length == 1)
				{
					eventBus.Register(genericArgs[0], new IocEventHandlerFactory(scopeFactory, @interface));
				}
			}
		}
	}
}