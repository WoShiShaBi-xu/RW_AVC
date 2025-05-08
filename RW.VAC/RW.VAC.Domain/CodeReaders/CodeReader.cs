using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;

namespace RW.VAC.Domain.CodeReaders;

[Table(Name = "Code_Reader")]
public class CodeReader : AggregateRoot<Guid>
{
	public CodeReader(string ip, string processName, ProcessType processType)
	{
		IP = ip;
		ProcessName = processName;
		ProcessType = processType;
	}

	public CodeReader(Guid id, string ip, string processName, ProcessType processType) : base(id)
	{
		IP = ip;
		ProcessName = processName;
		ProcessType = processType;
	}

	[Column(StringLength = 50)]
	// ReSharper disable once InconsistentNaming
	public string IP { get; set; }

	/// <summary>
	///		对应工序名称
	/// </summary>
	[Column(StringLength = 100)]
	public string ProcessName { get; set; }

	/// <summary>
	///		读编器处理类型
	/// </summary>
	public ProcessType ProcessType { get; set; }
}