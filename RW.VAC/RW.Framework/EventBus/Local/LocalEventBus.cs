using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using RW.Framework.Extensions;

namespace RW.Framework.EventBus.Local;

public class LocalEventBus : IEventBus
{
	private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories = new();

	public IDisposable Register<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData
	{
		return Register(typeof(TEventData), new ActionEventHandler<TEventData>(action));
	}

	public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
	{
		return Register(typeof(TEventData), handler);
	}

	public IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler, new()
	{
		return Register(typeof(TEventData), new TransientEventHandlerFactory<THandler>());
	}

	public IDisposable Register(Type eventType, IEventHandler handler)
	{
		return Register(eventType, new SingleInstanceHandlerFactory(handler));
	}

	public IDisposable Register(Type eventType, IEventHandlerFactory factory)
	{
		GetOrCreateHandlerFactories(eventType)
			.Locking(factories => factories.Add(factory));

		return new FactoryUnregistrar(this, eventType, factory);
	}

	public void Unregister<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData
	{
		if(action == null) throw new ArgumentNullException(nameof(action));
		GetOrCreateHandlerFactories(typeof(TEventData))
			.Locking(factories =>
			{
				factories.RemoveAll(
					factory =>
					{
						if (factory is not SingleInstanceHandlerFactory singleInstanceFactory)
						{
							return false;
						}

						if (singleInstanceFactory.HandlerInstance is not ActionEventHandler<TEventData> actionHandler)
						{
							return false;
						}

						return actionHandler.Action == action;
					});
			});
	}

	public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
	{
		Unregister(typeof(TEventData), handler);
	}

	public void Unregister(Type eventType, IEventHandler handler)
	{
		GetOrCreateHandlerFactories(eventType)
			.Locking(factories =>
			{
				factories.RemoveAll(
					factory =>
						factory is SingleInstanceHandlerFactory handlerFactory &&
						handlerFactory.HandlerInstance == handler);
			});
	}

	public void Unregister(Type eventType, IEventHandlerFactory factory)
	{
		GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
	}

	public void UnregisterAll<TEventData>() where TEventData : IEventData
	{
		UnregisterAll(typeof(TEventData));
	}

	public void UnregisterAll(Type eventType)
	{
		GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
	}

	public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
	{
		return TriggerAsync(typeof(TEventData), eventData);
	}

	public async Task TriggerAsync(Type eventType, IEventData eventData)
	{
		var exceptions = new List<Exception>();
		
		await new SynchronizationContextRemover();

		foreach (var handlerFactories in GetHandlerFactories(eventType).ToList())
		{
			foreach (var handlerFactory in handlerFactories.EventHandlerFactories.ToList())
			{
				await TriggerHandlerAsync(handlerFactory, handlerFactories.EventType, eventData, exceptions);
			}
		}

		if (exceptions.Any())
		{
			if (exceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
			}

			throw new AggregateException("More than one error has occurred while triggering the event: " + eventType, exceptions);
		}
	}

	protected async Task TriggerHandlerAsync(IEventHandlerFactory asyncHandlerFactory, Type eventType, IEventData eventData, List<Exception> exceptions)
	{
		using var eventHandlerWrapper = asyncHandlerFactory.GetHandler();
		try
		{
			var asyncHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
			var method = asyncHandlerType.GetMethod("HandleEventAsync", [eventType]);
			await (Task)method.Invoke(eventHandlerWrapper.EventHandler, [eventData]);
		}
		catch (TargetInvocationException ex)
		{
			exceptions.Add(ex.InnerException!);
		}
		catch (Exception ex)
		{
			exceptions.Add(ex);
		}
		
	}

	private IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
	{
		var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

		foreach (var handlerFactory in _handlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
		{
			handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
		}

		return handlerFactoryList.ToArray();
	}

	private static bool ShouldTriggerEventForHandler(Type eventType, Type handlerType)
	{
		//Should trigger same type
		if (handlerType == eventType)
		{
			return true;
		}

		//Should trigger for inherited types
		if (handlerType.IsAssignableFrom(eventType))
		{
			return true;
		}

		return false;
	}

	private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
	{
		return _handlerFactories.GetOrAdd(eventType, _ => new List<IEventHandlerFactory>());
	}

	private class EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
	{
		public Type EventType { get; } = eventType;

		public List<IEventHandlerFactory> EventHandlerFactories { get; } = eventHandlerFactories;
	}

	private struct SynchronizationContextRemover : INotifyCompletion
	{
		public bool IsCompleted => SynchronizationContext.Current == null;

		public void OnCompleted(Action continuation)
		{
			var prevContext = SynchronizationContext.Current;
			try
			{
				SynchronizationContext.SetSynchronizationContext(null);
				continuation();
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext(prevContext);
			}
		}

		public SynchronizationContextRemover GetAwaiter()
		{
			return this;
		}

		public void GetResult()
		{
		}
	}
}