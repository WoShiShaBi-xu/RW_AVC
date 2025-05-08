namespace RW.Framework.EventBus;

public interface IEventHandlerDisposeWrapper : IDisposable
{
	IEventHandler EventHandler { get; }
}