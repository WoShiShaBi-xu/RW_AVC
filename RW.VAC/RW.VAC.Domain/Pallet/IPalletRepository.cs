using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Pallet
{
    public interface IPalletRepository
    {
        /// <summary>
        /// 获取所有托盘
        /// </summary>
        /// <returns>托盘列表</returns>
        Task<IEnumerable<Pallet>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取托盘
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>托盘实体</returns>
        Task<Pallet> GetByIdAsync( string palletId );

        /// <summary>
        /// 根据类型获取托盘
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>托盘列表</returns>
        Task<IEnumerable<Pallet>> GetByTypeAsync( PalletType palletType );

        /// <summary>
        /// 根据状态获取托盘
        /// </summary>
        /// <param name="status">托盘状态</param>
        /// <returns>托盘列表</returns>
        Task<IEnumerable<Pallet>> GetByStatusAsync( string status );

        /// <summary>
        /// 根据位置获取托盘
        /// </summary>
        /// <param name="locationId">位置ID</param>
        /// <returns>托盘列表</returns>
        Task<IEnumerable<Pallet>> GetByLocationAsync( string locationId );

        /// <summary>
        /// 添加托盘
        /// </summary>
        /// <param name="pallet">托盘实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( Pallet pallet );

        /// <summary>
        /// 更新托盘
        /// </summary>
        /// <param name="pallet">托盘实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( Pallet pallet );

        /// <summary>
        /// 删除托盘
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( string palletId );
    }
}
