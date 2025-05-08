using System.Collections.Concurrent;

namespace RW.VAC.Infrastructure.Devices.State;

public class BaseState
{
    protected readonly ConcurrentDictionary<string, bool> Cache = new();

    public int Total => Cache.Count;

    public int Online => Cache.Count(x => x.Value);

    public bool this[string key]
    {
        set
        {
            Cache.AddOrUpdate(key, value, (_, _) => value);
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}