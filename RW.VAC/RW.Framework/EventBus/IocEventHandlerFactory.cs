using Microsoft.Extensions.DependencyInjection;

namespace RW.Framework.EventBus;

public class IocEventHandlerFactory(IServiceScopeFactory scopeFactory, Type handlerType)
	: IEventHandlerFactory, IDisposable
{
	public Type HandlerType { get; } = handlerType;

	public void Dispose()
	{
	}


	public IEventHandlerDisposeWrapper GetHandler()
	{
		var scope = scopeFactory.CreateScope();
		return new EventHandlerDisposeWrapper((IEventHandler) scope.ServiceProvider.GetRequiredService(HandlerType),
			() => scope.Dispose());
	}

	public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
	{
		return handlerFactories.OfType<IocEventHandlerFactory>().Any(f => f.HandlerType == HandlerType);
	}
}