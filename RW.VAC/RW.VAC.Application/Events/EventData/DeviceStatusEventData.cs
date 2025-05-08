using RW.Framework.EventBus;

namespace RW.VAC.Application.Events.EventData;

public class DeviceStatusEventData : IEventData
{
	public string DeviceCode { get; set; }

    public object StatusValue { get; set; }

	public string TagType { get; set; }

	public double Beat{ get; set; }
}