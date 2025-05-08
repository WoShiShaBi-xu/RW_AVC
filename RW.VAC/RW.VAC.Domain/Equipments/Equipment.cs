using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;

namespace RW.VAC.Domain.Equipments;

[Table(Name = "Equipment")]
public class Equipment : AggregateRoot<Guid>
{
	public Equipment(string name, string code, bool isAutomatic)
	{
		Name = name;
		Code = code;
		IsAutomatic = isAutomatic;
	}

	public Equipment(Guid id, string name, string code, bool isAutomatic) : base(id)
	{
		Name = name;
		Code = code;
		IsAutomatic = isAutomatic;
	}

	[Column(StringLength = 50)]
	public string Name { get; set; }

	[Column(StringLength = 50)]
	public string Code { get; set; }

	/// <summary>
	///		是否为自动化设备
	/// </summary>
	public bool IsAutomatic { get; set; }
}