using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Location
{
    public interface ILocationRepository
    {
        /// <summary>
        /// 获取所有库位
        /// </summary>
        /// <returns>库位列表</returns>
        Task<IEnumerable<Location>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取库位
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>库位实体</returns>
        Task<Location> GetByIdAsync( string locationId );

        /// <summary>
        /// 根据类型获取库位
        /// </summary>
        /// <param name="locationType">库位类型</param>
        /// <returns>库位列表</returns>
        Task<IEnumerable<Location>> GetByTypeAsync( LocationType locationType );

        /// <summary>
        /// 获取空闲库位
        /// </summary>
        /// <returns>空闲库位列表</returns>
        Task<IEnumerable<Location>> GetAvailableAsync( );

        /// <summary>
        /// 根据托盘ID获取库位
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>库位实体</returns>
        Task<Location> GetByPalletIdAsync( string palletId );

        /// <summary>
        /// 添加库位
        /// </summary>
        /// <param name="location">库位实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( Location location );

        /// <summary>
        /// 更新库位
        /// </summary>
        /// <param name="location">库位实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( Location location );

        /// <summary>
        /// 删除库位
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( string locationId );
    }
}
