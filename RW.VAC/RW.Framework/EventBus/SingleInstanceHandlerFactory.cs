namespace RW.Framework.EventBus;

public class SingleInstanceHandlerFactory(IEventHandler handlerInstance) : IEventHandlerFactory
{
	public IEventHandler HandlerInstance { get; } = handlerInstance;

	public IEventHandlerDisposeWrapper GetHandler()
	{
		return new EventHandlerDisposeWrapper(HandlerInstance);
	}

	public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
	{
		return handlerFactories
			.OfType<SingleInstanceHandlerFactory>()
			.Any(f => f.HandlerInstance == HandlerInstance);
	}
}