namespace RW.Framework.EventBus;

public class FactoryUnregistrar(IEventBus eventBus, Type eventType, IEventHandlerFactory factory)
	: IDisposable
{
	public void Dispose()
	{
		eventBus.Unregister(eventType, factory);
	}
}