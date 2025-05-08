namespace RW.VAC.Domain.CodeReaders;

public enum ProcessType : byte
{
    /// <summary>
    ///		通用
    /// </summary>
    General,

    /// <summary>
    ///		上料
    /// </summary>
    Feed,

    /// <summary>
    ///		叶片推杆紧固机
    /// </summary>
    Fastening,
    /// <summary>
    /// 试验台
    /// </summary>
    Bedstand,
    /// <summary>
    /// 开关试验台
    /// </summary>

    Switching,

	ArtificialBit

}