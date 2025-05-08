using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;

namespace RW.VAC.Domain.Records;

[Table(Name = "Production_Detail")]
public class ProductionDetail : AggregateRoot<Guid>
{
	public ProductionDetail(Guid recordId, string processName, DateTime startTime)
	{
		RecordId = recordId;
		ProcessName = processName;
		StartTime = startTime;
	}

	public ProductionDetail(Guid id, Guid recordId, string processName, DateTime startTime) : base(id)
	{
		RecordId = recordId;
		ProcessName = processName;
		StartTime = startTime;
	}

	public Guid RecordId { get; set; }

	[Column(StringLength = 100)]
	public string ProcessName { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public ProcessStatus Status { get; set; }

	public void SetAbnormalOffline()
	{
		if (Status == ProcessStatus.InProgress)
		{
			Status = ProcessStatus.AbnormalOffline;
		}
	}

	public void SetCompleted()
	{
		if (Status == ProcessStatus.Completed) return;
		EndTime = DateTime.Now;
		Status = ProcessStatus.Completed;
	}
}