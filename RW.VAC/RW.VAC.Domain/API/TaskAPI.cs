using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.API
{
    #region 任务查询相关模型

    /// <summary>
    /// 任务详情查询响应
    /// </summary>
    public class TaskDetailResponse
    {
        /// <summary>
        /// 返回码：0 成功，-1 失败
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 返回成功或者失败的信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 接口返回数据
        /// </summary>
        public TaskDetailData Data { get; set; }
    }

    /// <summary>
    /// 任务详情数据
    /// </summary>
    public class TaskDetailData
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string CustomTaskCode { get; set; }

        /// <summary>
        /// 自定义任务名称
        /// </summary>
        public string CustomName { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int JobPriority { get; set; } = 5;

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 子任务
        /// </summary>
        public List<SubTaskInfo> SubTaskList { get; set; }
    }
    public class GetVehicleResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public VehicleData Data { get; set; }
    }

    public class VehicleData
    {
        [JsonProperty("vehicleType")]
        public string VehicleType { get; set; }

        [JsonProperty("vehicleCode")]
        public string VehicleCode { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("vehicleStatus")]
        public string VehicleStatus { get; set; }

        [JsonProperty("onlineStatus")]
        public string OnlineStatus { get; set; }

        [JsonProperty("isMaintain")]
        public bool IsMaintain { get; set; }

        [JsonProperty("autoCharge")]
        public bool AutoCharge { get; set; }

        [JsonProperty("containers")]
        public object[] Containers { get; set; }

        [JsonProperty("locationCode")]
        public string LocationCode { get; set; }

        [JsonProperty("direction")]
        public int Direction { get; set; }

        [JsonProperty("battery")]
        public double Battery { get; set; }

        [JsonProperty("batteryVoltage")]
        public double BatteryVoltage { get; set; }

        [JsonProperty("batteryTemperature")]
        public double BatteryTemperature { get; set; }

        [JsonProperty("resourceGroupNames")]
        public string[] ResourceGroupNames { get; set; }
    }
    /// <summary>
    /// 子任务信息
    /// </summary>
    public class SubTaskInfo
    {
        /// <summary>
        /// 目标点编码
        /// </summary>
        public int LocationCode { get; set; }

        /// <summary>
        /// 载具
        /// </summary>
        public List<string> LoadCodes { get; set; }

        /// <summary>
        /// 储位
        /// </summary>
        public List<string> StorageCodes { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 任务失败原因（任务准备阶段无法执行才有）
        /// </summary>
        public List<TaskFailedReason> TaskFailedReasonList { get; set; }
    }

    /// <summary>
    /// 任务失败原因
    /// </summary>
    public class TaskFailedReason
    {
        /// <summary>
        /// 小车编码
        /// </summary>
        public string VehicleCode { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string TaskFailedReasons { get; set; }
    }

    #endregion

    #region 发送订单任务相关模型

    /// <summary>
    /// 订单任务
    /// </summary>
    public class OrderTask
    {
        /// <summary>
        /// 客户的订单编码，只做关联查询用，不查重 非必填
        /// </summary>
        public string CustomOrderCode { get; set; }

        /// <summary>
        /// 订单模式，对应不同流程， 非必填,默认普通模式
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// 订单载具
        /// </summary>
        public List<OrderLoad> OrderLoads { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public int? BatchNumber { get; set; }

        /// <summary>
        /// 批次内顺序，小的执行完才可以执行大的， 非必填
        /// </summary>
        public int? BatchSortNumber { get; set; }

        /// <summary>
        /// 依赖批次 非必填
        /// </summary>
        public int? DepBatchNumber { get; set; }

        /// <summary>
        /// 订单截止时间，尽最大努力在此时间前完成任务 timestamp 非必填
        /// </summary>
        public long? CutOffTime { get; set; }

        /// <summary>
        /// 优先级，尽量优先执行, 非必填
        /// </summary>
        public int Priority { get; set; } = 5;
    }

    /// <summary>
    /// 订单载具
    /// </summary>
    public class OrderLoad
    {
        /// <summary>
        /// 起始地址,非必填
        /// </summary>
        public Location Source { get; set; }

        /// <summary>
        /// 目标地址,必填
        /// </summary>
        public Location Target { get; set; }

        /// <summary>
        /// 订单涉及的载具
        /// </summary>
        public List<Load> Loads { get; set; }

        /// <summary>
        /// 到点顺序, 非必填
        /// </summary>
        public int? TargetArrivalSequence { get; set; }
    }

    /// <summary>
    /// 地址位置
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 点位编码
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 点位类型
        /// </summary>
        public string LocationType { get; set; }
    }

    /// <summary>
    /// 载具
    /// </summary>
    public class Load
    {
        /// <summary>
        /// 载具编码
        /// </summary>
        public string LoadCode { get; set; }

        /// <summary>
        /// 载具类型
        /// </summary>
        public string LoadType { get; set; }

        /// <summary>
        /// 载具角度
        /// </summary>
        public int? LoadAngle { get; set; }

        /// <summary>
        /// 自定义任务编码
        /// </summary>
        public string CustomTaskCode { get; set; }
    }

    /// <summary>
    /// 发送订单任务响应
    /// </summary>
    public class SendOrderTasksResponse
    {
        /// <summary>
        /// 返回码：0 成功，-1 失败
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 返回成功或者失败的信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 接口返回数据
        /// </summary>
        public List<OrderTaskResult> Data { get; set; }
    }
    public class SendOrderTasksResponse1
    {
        /// <summary>
        /// 返回码：0 成功，-1 失败
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 返回成功或者失败的信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 接口返回数据
        /// </summary>
        public AddLoadResponseData Data { get; set; }
    }
    public class AddLoadResponseData
    {
        /// <summary>
        /// 载具点位
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 载具编码
        /// </summary>
        public string LoadCode { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Disabled { get; set; } = false;
    }
    /// <summary>
    /// 订单任务结果
    /// </summary>
    public class OrderTaskResult
    {
        /// <summary>
        /// 订单唯一ID，如果客户提供，用客户提供的，但是会判断重复
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 客户的订单编码，只做关联查询用，不查重
        /// </summary>
        public string CustomOrderCode { get; set; }

        /// <summary>
        /// 订单状态返回code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 订单状态返回信息
        /// </summary>
        public string Message { get; set; }
    }

    #endregion

    #region 枚举定义

    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        New,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing,
        /// <summary>
        /// 申请重新计算执行中
        /// </summary>
        AskReExecuting,
        /// <summary>
        /// 重新计算执行中
        /// </summary>
        ReExecuting,
        /// <summary>
        /// 重新load路径中
        /// </summary>
        ReLoad,
        /// <summary>
        /// 取消中
        /// </summary>
        Cancelling,
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 停止
        /// </summary>
        Stop,
        /// <summary>
        /// Charging
        /// </summary>
        Charging,
        /// <summary>
        /// 准备执行状态
        /// </summary>
        Prepare,
        /// <summary>
        /// 等待命令状态
        /// </summary>
        Wait,
        /// <summary>
        /// 异常
        /// </summary>
        Error,
        /// <summary>
        /// 完成
        /// </summary>
        Finished
    }

    #endregion

    #region 载具信息模型
    /// <summary>
    /// 载具信息模型
    /// </summary>
    public class LoadInfo
    {
        [JsonProperty( "loadType" )]
        public string LoadType { get; set; }

        [JsonProperty( "loadTypeName" )]
        public string LoadTypeName { get; set; }

        [JsonProperty( "loadCode" )]
        public string LoadCode { get; set; }

        [JsonProperty( "length" )]
        public int Length { get; set; }

        [JsonProperty( "width" )]
        public int Width { get; set; }

        [JsonProperty( "height" )]
        public int Height { get; set; }

        [JsonProperty( "storageCode" )]
        public string StorageCode { get; set; }

        [JsonProperty( "locationCode" )]
        public string LocationCode { get; set; }

        [JsonProperty( "angle" )]
        public int Angle { get; set; }

        [JsonProperty( "disabled" )]
        public bool Disabled { get; set; }

        [JsonProperty( "resourceGroups" )]
        public string [ ] ResourceGroups { get; set; }

        [JsonProperty( "createTime" )]
        public string CreateTime { get; set; }

        [JsonProperty( "updateTime" )]
        public string UpdateTime { get; set; }
    }

    /// <summary>
    /// 查询所有载具响应模型
    /// </summary>
    public class GetLoadsResponse
    {
        [JsonProperty( "code" )]
        public string Code { get; set; }

        [JsonProperty( "message" )]
        public string Message { get; set; }

        [JsonProperty( "data" )]
        public LoadInfo [ ] Data { get; set; }
    }
    #endregion
}
