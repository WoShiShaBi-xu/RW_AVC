using FreeSql;

namespace RW.Framework.Extensions;

public static class FreeSqlExtension
{
	public static ISelect<TEntity> OrderByPropertyList<TEntity>(this ISelect<TEntity> select, List<(string, bool)>? order)
	{
		if (order == null || order.Count == 0) return select;
		return order.Aggregate(select, (current, item) => current.OrderByPropertyName(item.Item1, item.Item2));
	}
}