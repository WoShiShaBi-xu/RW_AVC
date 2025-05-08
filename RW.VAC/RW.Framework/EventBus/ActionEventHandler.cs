namespace RW.Framework.EventBus;

public class ActionEventHandler<TEventData>(Func<TEventData, Task> action) : IEventHandler<TEventData>
{
	public Func<TEventData, Task> Action { get; } = action;

	public async Task HandleEventAsync(TEventData eventData)
	{
		await Action(eventData);
	}
}