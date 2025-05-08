using CircularBuffer;
using RW.VAC.Client.Models;

namespace RW.VAC.Client.Storage;

public class LogBuffer
{
    private static readonly object Locker = new();

    public CircularBuffer<LogMessage> Storage { get; private set; } = null!;

    public void Initialize(int? capacity)
    {
        lock (Locker)
        {
            Storage = new CircularBuffer<LogMessage>(capacity ?? 50);
        }
    }
}