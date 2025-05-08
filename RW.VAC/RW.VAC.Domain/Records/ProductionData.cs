using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;

namespace RW.VAC.Domain.Records;

[Table(Name = "Production_Data")]
public class ProductionData: AggregateRoot<Guid>
{
	public ProductionData(Guid detailId, string name, string value, string? unit = null)
	{
		DetailId = detailId;
		Name = name;
		Value = value;
		Unit = unit;
	}

	public ProductionData(Guid id, Guid detailId, string name, string value, string? unit = null) : base(id)
	{
		DetailId = detailId;
		Name = name;
		Value = value;
		Unit = unit;
	}

	public Guid DetailId { get; set; }

	[Column(StringLength = 100)]
	public string Name { get; set; }

	[Column(StringLength = 100)]
	public string Value { get; set; }

	[Column(StringLength = 20)]
	public string? Unit { get; set; }
}