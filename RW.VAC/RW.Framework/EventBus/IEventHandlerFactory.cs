namespace RW.Framework.EventBus;

public interface IEventHandlerFactory
{
	IEventHandlerDisposeWrapper GetHandler();

	bool IsInFactories(List<IEventHandlerFactory> handlerFactories);
}