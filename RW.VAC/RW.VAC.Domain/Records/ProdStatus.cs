using System.ComponentModel;

namespace RW.VAC.Domain.Records;

public enum ProdStatus
{
    [Description("生产中")]
    InProgress,

    [Description("异常下线")]
    AbnormalOffline,

    [Description("重新上线")]
    BackOnline,

    [Description("完成")]
    Completed,
}