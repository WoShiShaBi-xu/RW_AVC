using RW.Framework.EventBus;
using RW.VAC.Application.Events.EventData;
using Serilog.Core;
using Serilog.Events;

namespace RW.VAC.Client.LogSink;

public class AppClientSink(IEventBus eventBus) : ILogEventSink
{
	public void Emit(LogEvent logEvent)
	{
		logEvent.Properties.TryGetValue(Constants.SourceContextPropertyName, out var propertyValue);
		var source = propertyValue?.ToString().Replace("\"", string.Empty) ?? string.Empty;
		var data = new LogEventData(logEvent.Timestamp, logEvent.Level.ToString().ToLower(),
			source[(source.LastIndexOf(".", StringComparison.Ordinal) + 1)..],
			logEvent.RenderMessage());
		eventBus.TriggerAsync(data).Wait(0);
	}
}