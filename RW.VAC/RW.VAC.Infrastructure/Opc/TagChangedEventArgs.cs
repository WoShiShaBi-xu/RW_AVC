using Opc.Ua;
using Opc.Ua.Client;

namespace RW.VAC.Infrastructure.Opc;

public class TagChangedEventArgs(string? groupCode, string? itemCode, MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs args) : EventArgs
{
	public MonitoredItem MonitoredItem { get; } = monitoredItem;

	public MonitoredItemNotificationEventArgs Args { get; } = args;

	public object Value => (Args.NotificationValue as MonitoredItemNotification)!.Value.WrappedValue.Value;

	public string? GroupCode { get; } = groupCode;

	public string? ItemCode { get; } = itemCode;

    public bool IsBeat => ItemCode == "Beat";

}