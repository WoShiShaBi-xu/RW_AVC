namespace RW.VAC.Client.Models;

public class LogMessage(string timestamp, string level, string source, string message)
{
	public string Timestamp { get; set; } = timestamp;

	public string Level { get; set; } = level;

	public string Source { get; set; } = source;

	public string Message { get; set; } = message;
}