using System.ComponentModel;

namespace RW.VAC.Domain.Records;

public enum ProcessStatus
{
	[Description("生产中")]
	InProgress,

	[Description("异常下线")]
	AbnormalOffline,

	[Description("完成")]
	Completed,
}