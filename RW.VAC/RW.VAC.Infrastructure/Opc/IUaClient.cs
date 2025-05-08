using System.Collections.ObjectModel;

namespace RW.VAC.Infrastructure.Opc;

public interface IUaClient
{
	/// <summary>
	///		连接OPC UA
	/// </summary>
	/// <param name="url">连接Url</param>
	/// <param name="clientId">客户端Id</param>
	/// <returns></returns>
	Task Connect(string url, string clientId = "rw.client");

	/// <summary>
	///     订阅
	/// </summary>
	/// <param name="nodeId">节点Id</param>
	/// <param name="name">订阅名</param>
	/// <param name="ignoreInitial">是否忽略首次订阅事件</param>
	/// <param name="action">订阅事件</param>
	void Subscribe(string nodeId, string? name = null, bool ignoreInitial = false,
		Action<TagChangedEventArgs>? action = null);

	/// <summary>
	///		订阅
	/// </summary>
	/// <param name="nodeIds">节点Id</param>
	/// <param name="name">订阅名</param>
	/// <param name="ignoreInitial">是否忽略首次订阅事件</param>
	/// <param name="action">订阅事件</param>
	void Subscribe(List<string> nodeIds, string? name = null, bool ignoreInitial = false,
		Action<TagChangedEventArgs>? action = null);

	/// <summary>
	///		取消订阅
	/// </summary>
	/// <param name="name">订阅名</param>
	void Unsubscribe(string name);

	/// <summary>
	///		取消订阅
	/// </summary>
	/// <param name="name">订阅名</param>
	/// <param name="nodes">节点Id</param>
	void Unsubscribe(string name, params string[] nodes);

	/// <summary>
	///		取消全部订阅
	/// </summary>
	void UnsubscribeAll();

	/// <summary>
	///		读取节点
	/// </summary>
	/// <param name="nodeId">节点Id</param>
	/// <returns></returns>
	object? Read(string nodeId);

	/// <summary>
	///		读取节点
	/// </summary>
	/// <param name="nodeId">节点Id</param>
	/// <returns></returns>
	T? Read<T>(string nodeId);

	/// <summary>
	///		读取节点
	/// </summary>
	/// <typeparam name="T">数据类型</typeparam>
	/// <param name="nodes">节点</param>
	/// <param name="defaultValue">默认值</param>
	/// <returns></returns>
	IEnumerable<T?>? Read<T>(IEnumerable<string> nodes, T? defaultValue = default);

	/// <summary>
	///		写入节点
	/// </summary>
	/// <param name="nodeId">节点Id</param>
	/// <param name="value">写入值</param>
	/// <returns></returns>
	bool Write(string nodeId, object value);

	/// <summary>
	///		批量写入节点
	/// </summary>
	/// <param name="nodes">节点Id</param>
	/// <param name="value">写入值</param>
	/// <returns></returns>
	bool Write(IEnumerable<string> nodes, object value);

	/// <summary>
	///		关闭连接
	/// </summary>
	void Close();

	/// <summary>
	///		添加订阅事件
	/// </summary>
	/// <param name="name">订阅名称</param>
	/// <param name="action">事件</param>
	void AddAction(string name, Action<TagChangedEventArgs>? action);

	/// <summary>
	///		移除订阅事件
	/// </summary>
	/// <param name="name">订阅名称</param>
	/// <param name="action">事件</param>
	void RemoveAction(string name, Action<TagChangedEventArgs> action);
}