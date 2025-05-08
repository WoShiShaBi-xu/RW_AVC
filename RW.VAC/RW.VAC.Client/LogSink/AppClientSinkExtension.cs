using RW.Framework.EventBus;
using Serilog;
using Serilog.Configuration;

namespace RW.VAC.Client.LogSink;

public static class AppClientSinkExtension
{
	public static LoggerConfiguration AppClientSink(this LoggerSinkConfiguration loggerConfiguration, IEventBus eventBus)
	{
		return loggerConfiguration.Sink(new AppClientSink(eventBus));
	}
}