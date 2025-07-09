using RW.VAC.Domain.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Client.Models
{
    public class LocationEditModel
    {
        public string LocationId { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public LocationType LocationType { get; set; } = LocationType.缓存区_待试验;
    }

    public class DockingPositionEditModel
    {
        public string PositionId { get; set; } = string.Empty;
        public string PositionType { get; set; } = string.Empty;
        public string StationId { get; set; } = string.Empty;
    }

    public class AssignPalletModalModel
    {
        public string LocationId { get; set; } = string.Empty;
        public string PalletId { get; set; } = string.Empty;
        public Domain.Pallet.PalletType PalletType { get; set; } = Domain.Pallet.PalletType.制动装置托盘;
    }

    public class AssignDockingPalletModalModel
    {
        public string PositionId { get; set; } = string.Empty;
        public string PalletId { get; set; } = string.Empty;
        public Domain.Pallet.PalletType PalletType { get; set; } = Domain.Pallet.PalletType.制动装置托盘;
    }
    /// <summary>
    /// 产品分配模态框数据模型
    /// </summary>
    public class AssignProductModalModel
    {
        /// <summary>
        /// 库位ID
        /// </summary>
        [Required( ErrorMessage = "库位ID不能为空" )]
        public string LocationId { get; set; }

        /// <summary>
        /// 选择的产品ID
        /// </summary>
        [Required( ErrorMessage = "请选择产品" )]
        public string ProductId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
    }

    /// <summary>
    /// 接驳位产品分配模态框数据模型
    /// </summary>
    public class AssignDockingProductModalModel
    {
        /// <summary>
        /// 接驳位ID
        /// </summary>
        [Required( ErrorMessage = "接驳位ID不能为空" )]
        public string PositionId { get; set; }

        /// <summary>
        /// 选择的产品ID
        /// </summary>
        [Required( ErrorMessage = "请选择产品" )]
        public string ProductId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
    }

}