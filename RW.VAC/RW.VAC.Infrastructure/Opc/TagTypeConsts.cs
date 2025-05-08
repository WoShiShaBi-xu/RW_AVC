namespace RW.VAC.Infrastructure.Opc;

public static class TagTypeConsts
{
	public const string StatusTag = "Status";//状态

	public const string LackTag = "Lack";//缺料

	public const string OverLimitTag = "NGOverLimit";//超限

	public const string HeartbeatTag = "Heartbeat";//心跳

	public const string WriteRecipeTag = "WriteRecipe";//写入配方号

    public const string StartTag = "Start";//启动

	public const string PauseTag = "Pause";//暂停

	public const string ResetTag = "Reset";//重置

	public const string RemoteModeTag = "RemoteMode";//远程模式

	public const string EmergencyStopTag = "EmergencyStop";//急停

	public const string ProductivityTag = "Productivity";//产量

	public const string BeatTag = "Beat";//生产节拍

	public const string NGCountTag = "NGCount";//NG数量

    public const string EndofprocessTag = "Endofprocess";//工序结束

    public const string BlankingTag = "Blanking";//下料工位号

	public const string ScanningDataTag = "ScanningData";//扫码枪数据

	public const string SNTag = "SN";//SN

	public const string InboundTimeTag = "InboundTime";//入站时间

	public const string OutboundTimeTag = "OutboundTime";//出站时间

}