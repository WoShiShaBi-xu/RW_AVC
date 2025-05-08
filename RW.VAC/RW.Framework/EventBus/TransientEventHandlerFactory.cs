namespace RW.Framework.EventBus;

public class TransientEventHandlerFactory(Type handlerType) : IEventHandlerFactory
{
	public Type HandlerType { get; } = handlerType;

	public IEventHandlerDisposeWrapper GetHandler()
	{
		var handler = CreateHandler();
		return new EventHandlerDisposeWrapper(
			handler,
			() => (handler as IDisposable)?.Dispose()
		);
	}

	public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
	{
		return handlerFactories
			.OfType<TransientEventHandlerFactory>()
			.Any(f => f.HandlerType == HandlerType);
	}

	protected virtual IEventHandler CreateHandler()
	{
		return (IEventHandler)Activator.CreateInstance(HandlerType)!;
	}
}

public class TransientEventHandlerFactory<THandler>() : TransientEventHandlerFactory(typeof(THandler))
	where THandler : IEventHandler, new()
{
	protected override IEventHandler CreateHandler()
	{
		return new THandler();
	}
}