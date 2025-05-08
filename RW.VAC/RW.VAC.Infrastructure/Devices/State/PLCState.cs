namespace RW.VAC.Infrastructure.Devices.State;

// ReSharper disable once InconsistentNaming
public class PLCState : BaseState
{
    public List<string> Tags => Cache.Keys.ToList();
}