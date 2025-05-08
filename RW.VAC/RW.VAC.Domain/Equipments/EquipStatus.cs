using System.ComponentModel;

namespace RW.VAC.Domain.Equipments;

public enum EquipStatus : ushort
{
	[Description("准备")] Preparing = 0,

	[Description("待机")] StandBy = 1,

	[Description("运行")] Running = 2,

	[Description("故障")] Alarm = 3,

	[Description("未知")] Unknown = 4
}