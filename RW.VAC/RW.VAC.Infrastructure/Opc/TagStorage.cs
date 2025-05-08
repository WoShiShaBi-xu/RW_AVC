
using System.Collections.Concurrent;
using RW.Framework.Extensions;

namespace RW.VAC.Infrastructure.Opc;

public class TagStorage
{
	private readonly ConcurrentDictionary<string, TagItem> _storage = new();

	private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, TagItem>> _groupCache = new();

	private readonly List<(string GroupCode, string ItemCode, string TagName)> _tagList = new();

	public IReadOnlyList<(string GroupCode, string ItemCode, string TagName)> TagList => _tagList.AsReadOnly();

	public TagItem? this[string group, string code]
	{
		get
		{
			var groupData = _groupCache.GetValueOrDefault(group);
			return groupData?.GetValueOrDefault(code);
		}
	}

	public TagItem? this[string tagName] => _storage.GetValueOrDefault(tagName);

	public void AddTag(string groupCode, string itemCode, string tagName, IUaClient uaClient)
	{
		var item = new TagItem(groupCode, itemCode, tagName, uaClient);
		_storage.TryAdd(tagName, item);
		_tagList.Locking(l => l.Add((groupCode, itemCode, tagName)));
	}

	public void Initialize()
	{
		foreach (var tagItem in _storage.Values)
		{
			var dicGroup = _groupCache.GetOrAdd(tagItem.GroupCode, _ => new ConcurrentDictionary<string, TagItem>());
			dicGroup.TryAdd(tagItem.ItemCode, tagItem);
		}
	}
}