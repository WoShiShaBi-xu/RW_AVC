using RW.Framework.EventBus;

namespace RW.VAC.Application.Events.EventData;

public class LogEventData(DateTimeOffset timestamp, string level, string source, string message) : IEventData
{
	public DateTimeOffset Timestamp { get; set; } = timestamp;

	public string Level { get; set; } = level;

	public string Source { get; set; } = source;

	public string Message { get; set; } = message;
}