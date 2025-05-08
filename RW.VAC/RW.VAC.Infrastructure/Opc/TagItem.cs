using RW.Framework.Extensions;

namespace RW.VAC.Infrastructure.Opc;

public class TagItem(string groupCode, string itemCode, string tagName, IUaClient uaClient)
{
	/// <summary>
	///		分组编码
	/// </summary>
	public string GroupCode { get; } = groupCode;

	/// <summary>
	///		点位编码
	/// </summary>
	public string ItemCode { get; } = itemCode;

	/// <summary>
	///		标记名称
	/// </summary>
	public string TagName { get; } = tagName;

	/// <summary>
	///		订阅名称
	/// </summary>
	public string? SubscriptionName { get; set; }

	/// <summary>
	///		是否订阅
	/// </summary>
	public bool Subscribed => SubscriptionName.NotNullOrWhiteSpace();

	/// <summary>
	///     忽略首次执行
	/// </summary>
	public bool IgnoreInitial { get; set; }

	/// <summary>
	///		是否首次执行
	/// </summary>
	public bool FirstExecuted { get; set; }

	public object? Value
	{
		get => uaClient.Read(TagName);
		set
		{
			if (value != null)
			{
				uaClient.Write(TagName, value);
			}
		}
	}

	/// <summary>
	///		显示名称
	///		指定标签取消订阅时，使用的是DisplayName
	/// </summary>
	public string? DisplayName { get; set; }

	public event Action<TagChangedEventArgs> Changed
	{
		add
		{
			if (!Subscribed)
			{
				uaClient.Subscribe(TagName);
			}
			uaClient.AddAction(SubscriptionName!, value);
		}
		remove
		{
			uaClient.RemoveAction(SubscriptionName!, value);
			if (Subscribed)
			{
				uaClient.Unsubscribe(SubscriptionName!, TagName);
			}
		}
	}
}