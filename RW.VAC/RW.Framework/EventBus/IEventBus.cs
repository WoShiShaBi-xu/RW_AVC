namespace RW.Framework.EventBus;

public interface IEventBus
{
	IDisposable Register<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData;

	IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

	IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler, new();

	IDisposable Register(Type eventType, IEventHandler handler);

	IDisposable Register(Type eventType, IEventHandlerFactory factory);

	void Unregister<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData;

	void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

	void Unregister(Type eventType, IEventHandler handler);

	void Unregister(Type eventType, IEventHandlerFactory factory);

	void UnregisterAll<TEventData>() where TEventData : IEventData;

	void UnregisterAll(Type eventType);

	Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;

	Task TriggerAsync(Type eventType, IEventData eventData);
}