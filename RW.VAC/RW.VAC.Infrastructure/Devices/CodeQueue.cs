using System.Collections.Concurrent;

namespace RW.VAC.Infrastructure.Devices;

public class CodeQueue
{
	private readonly ConcurrentQueue<string> _readerQueue = new();

	private readonly ConcurrentQueue<string> _deviceQueue = new();

	public void Entry(string code)
	{
		_readerQueue.Enqueue(code);
	}

	public string? Move()
	{
		if (!_readerQueue.TryDequeue(out var code)) return default;
		_deviceQueue.Enqueue(code);
		return code;
	}

	public string? Leave()
	{
		return !_deviceQueue.TryDequeue(out var code) ? default : code;
	}
}