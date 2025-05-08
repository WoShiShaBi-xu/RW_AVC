using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities.Auditing;

namespace RW.VAC.Domain.Parameters;

[Table(Name = "Parameter")]
public class Parameter : FullAuditedAggregateRoot<Guid>
{
	public Parameter(string code, string value, string? description)
	{
		Code = code;
		Value = value;
		Description = description;
	}

	public Parameter(Guid id, string code, string value, string? description) : base(id)
	{
		Code = code;
		Value = value;
		Description = description;
	}

	[Column(StringLength = 50)]  public string Code { get; set; }

	[Column(StringLength = 500)] public string Value { get; set; }

	[Column(StringLength = 100)] public string? Description { get; set; }

	public void Update(string code, string value, string? description)
	{
		this.Code = code;
		this.Value = value;
		this.Description = description;
	}
}