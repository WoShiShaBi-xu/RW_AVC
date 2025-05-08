using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities.Auditing;

namespace RW.VAC.Domain.Products;

[Table(Name = "Product")]
public class Product : FullAuditedAggregateRoot<Guid>
{
	public Product(string name, string code, RoutingTypes routing, int? recipe, string? statisticType )
	{
		Name = name;
		Code = code;
		Routing = routing;
		Recipe = recipe;
        StatisticType = statisticType;
    }

	public Product(Guid id, string name, string code, RoutingTypes routing, int? recipe,string? statisticType ) : base(id)
	{
		Name = name;
		Code = code;
		Routing = routing;
		Recipe = recipe;
        StatisticType=statisticType;

    }

	[Column(StringLength = 50)] public string Name { get; set; }

	[Column(StringLength = 50)] public string Code { get; set; }

	public int? Recipe { get; set; }

    public string? StatisticType { get; set; }

    public RoutingTypes Routing { get; set; }

	public void Update(string name, string code, RoutingTypes routing, int? recipe)
	{
		this.Name = name;
		this.Code = code;
		this.Routing = routing;
		this.Recipe = recipe;
	}
}