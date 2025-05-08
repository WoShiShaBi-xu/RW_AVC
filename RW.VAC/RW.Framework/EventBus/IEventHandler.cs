namespace RW.Framework.EventBus;

public interface IEventHandler
{
}

public interface IEventHandler<in TEventData> : IEventHandler
{
	Task HandleEventAsync(TEventData eventData);
}