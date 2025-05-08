using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities.Auditing;
using System.Diagnostics;

namespace RW.VAC.Domain.Opcs;

[Table(Name = "Opc_Item")]
public class OpcItem : FullAuditedEntity<Guid>
{
	public OpcItem(Guid groupId, string code, string name, string? description)
	{
		GroupId = groupId;
		Code = code;
		Name = name;
		Description = description;
	}

	public OpcItem(Guid id, Guid groupId, string code, string name, string? description,bool isProcess) : base(id)
	{
		GroupId = groupId;
		Code = code;
		Name = name;
		Description = description;
        IsProcess = isProcess;
    }

	public Guid GroupId { get; set; }

	[Column(StringLength = 50)] public string Code { get; set; }

	[Column(StringLength = 50)] public string Name { get; set; }

	[Column(StringLength = 100)] public string? Description { get; set; }
	/// <summary>
	/// 是否是工艺参数
	/// </summary>
    public bool IsProcess { get; set; }


    public void Update(string code, string name, string? description,bool isProcess)
	{
		Code = code;
		Name = name;
		Description = description;
		IsProcess = isProcess;
	}
}