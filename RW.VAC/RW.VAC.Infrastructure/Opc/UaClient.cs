using System.Collections.Concurrent;
using GodSharp.Opc.Ua;
using GodSharp.Opc.Ua.Client;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using RW.Framework.Extensions;

namespace RW.VAC.Infrastructure.Opc;

public class UaClient(ILogger<UaClient> logger, TagStorage tags) : IUaClient
{
	/// <summary>
	///     订阅事件缓存
	/// </summary>
	private readonly ConcurrentDictionary<string, TagChangedEventWrapper> _actionCache = new();

	private readonly ManualResetEventSlim _slim = new(false);

	private OpcUaClient? _client;

	public async Task Connect(string url, string clientId = "rw.client")
	{

		var builder = new OpcUaClientBuilder()
			.WithEndpoint(url)
			.WithAnonymous()
			.WithClientId(clientId);
		_client = await builder.BuildAsync();
		_client.OnMonitoredItemNotification += (n, i, e) =>
		{
			if (e.NotificationValue is MonitoredItemNotification notification&&StatusCode.IsBad(notification.Value.StatusCode))
			{
				return;
			}
			var tag = tags[i.StartNodeId.ToString()];
			if (tag is {IgnoreInitial: true, FirstExecuted: false})
			{
				tag.FirstExecuted = true;
				return;
			}
			
			var action = _actionCache.GetValueOrDefault(n);
            action?.OnChanged(new TagChangedEventArgs(tag?.GroupCode, tag?.ItemCode, i, e));
			
		};
		try
		{
			_client.Open();
			_slim.Set();
		}
		catch (Exception e)
		{
			logger.LogError(e, e.Message);
			throw;
		}

	}

	public void Subscribe(string nodeId, string? name = null, bool ignoreInitial = false, Action<TagChangedEventArgs>? action = null)
	{
		Subscribe([nodeId], name, ignoreInitial, action);
	}

	public void Subscribe(List<string> nodeIds, string? name = null, bool ignoreInitial = false, Action<TagChangedEventArgs>? action = null)
	{
		_slim.Wait();
		if (string.IsNullOrWhiteSpace(name)) name = Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();
		var nodeList = new List<NodeId>();
		foreach (var id in nodeIds)
		{
			var nodeId = new NodeId(id);
			nodeList.Add(nodeId);
			var item = tags[id];
			if (item == null) continue;
			item.SubscriptionName = name;
			item.IgnoreInitial = ignoreInitial;
			item.DisplayName = nodeId.Identifier.ToString();
		}
		_client?.Subscribe(name, nodeList);
		AddAction(name, action);
	}

	public void Unsubscribe(string name)
	{
		_client?.Unsubscribe(name);
		_actionCache.TryRemove(name, out _);
	}

	public void Unsubscribe(string name, params string[] nodes)
	{
		var displayNames = new List<string>();
		foreach (var nodeId in nodes)
		{
			var item = tags[nodeId];
			if (item == null) continue;
			item.SubscriptionName = null;
			displayNames.Add(item.DisplayName!);
		}
		_client?.Unsubscribe(name, displayNames.ToArray());
		var subscription = _client?.Session.Subscriptions.FirstOrDefault(t => t.DisplayName == name);
		if (subscription is {MonitoredItemCount: 0}) Unsubscribe(name);
	}

	public void UnsubscribeAll()
	{
		var names = _client?.Session.Subscriptions.Select(t => t.DisplayName).ToList();
		if (names == null) return;
		foreach (var name in names.Where(name => name.NotNullOrWhiteSpace()))
		{
			_client?.Unsubscribe(name);
		}
		_actionCache.Clear();
	}

	public object? Read(string nodeId)
	{
		return _client!.Read(nodeId)?.WrappedValue.Value;
	}

	public T? Read<T>(string nodeId)
	{
		return _client!.Read<T>(nodeId);
	}

	public IEnumerable<T?>? Read<T>(IEnumerable<string> nodes, T? defaultValue = default)
	{
		return _client!.Read(nodes, defaultValue);
	}

	public bool Write(string nodeId, object value)
	{
		return _client!.Write(nodeId, value);
	}

	public bool Write(IEnumerable<string> nodes, object value)
	{
		var toWrite = nodes.Select(node => (node, value)).ToArray();
		return _client!.Write(toWrite);
	}

	public void Close()
	{
		UnsubscribeAll();
		_client?.Close();
		_client?.Dispose();
	}

	public void AddAction(string name, Action<TagChangedEventArgs>? action)
	{
		if (action == null) return;
		var wrapper = _actionCache.GetOrAdd(name, _ => new TagChangedEventWrapper());
		wrapper.Changed += action;
	}

	public void RemoveAction(string name, Action<TagChangedEventArgs> action)
	{
		if (_actionCache.TryGetValue(name, out var wrapper))
		{
			wrapper.Changed -= action;
		}
	}
}