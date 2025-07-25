using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.AGV
{
    /// <summary>
    /// AGV上料指令请求模型
    /// </summary>
    public class LoadCommandRequest
    {
        /// <summary>
        /// 工位ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public string PalletId { get; set; }
    }

    /// <summary>
    /// AGV下料指令请求模型
    /// </summary>
    public class UnloadCommandRequest
    {
        /// <summary>
        /// 工位ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public string PalletId { get; set; }
    }

    /// <summary>
    /// AGV运输任务请求模型
    /// </summary>
    public class TransportTaskRequest
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        public string SourceLocation { get; set; }

        /// <summary>
        /// 目标位置
        /// </summary>
        public string TargetLocation { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public string PalletId { get; set; }
    }

    /// <summary>
    /// API响应模型
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
