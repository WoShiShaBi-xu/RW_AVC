using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities.Auditing;

namespace RW.VAC.Domain.Opcs;

[Table(Name = "Opc_Group")]
public class OpcGroup : FullAuditedAggregateRoot<Guid>
{
	public OpcGroup(string name, string code, string device, string? group)
	{
		Name = name;
		Code = code;
		Device = device;
		Group = group;
	}

	public OpcGroup(Guid id, string name, string code, string device, string? group) : base(id)
	{
		Name = name;
		Code = code;
		Device = device;
		Group = group;
	}

	[Column(StringLength = 50)] public string Name { get; set; }

	[Column(StringLength = 50)] public string Code { get; set; }

	[Column(StringLength = 100)] public string Device { get; set; }

	[Column(StringLength = 50)] public string? Group { get; set; }

	public void Update(string name, string code, string device, string? group)
	{
		this.Name = name;
		this.Code = code;
		this.Device = device;
		this.Group = group;
	}
}