using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Pallet
{
    public interface IPalletService
    {
        /// <summary>
        /// 创建新托盘
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>创建的托盘</returns>
        Task<Pallet> CreatePalletAsync( PalletType palletType );

        /// <summary>
        /// 更新托盘状态
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdatePalletStatusAsync( string palletId , string status );

        /// <summary>
        /// 更新托盘位置
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <param name="locationId">位置ID</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdatePalletLocationAsync( string palletId , string locationId );

        /// <summary>
        /// 获取托盘详情
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>托盘详情</returns>
        Task<Pallet> GetPalletDetailsAsync( string palletId );

        /// <summary>
        /// 获取所有托盘
        /// </summary>
        /// <returns>托盘列表</returns>
        Task<IEnumerable<Pallet>> GetAllAsync( );
    }
}
