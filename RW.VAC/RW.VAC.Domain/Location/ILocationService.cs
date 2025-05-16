using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Location
{
    public interface ILocationService
    {
        /// <summary>
        /// 创建新库位
        /// </summary>
        /// <param name="locationType">库位类型</param>
        /// <param name="locationName">库位名称</param>
        /// <returns>创建的库位</returns>
        Task<Location> CreateLocationAsync( LocationType locationType , string locationName );

        /// <summary>
        /// 更新库位状态
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <param name="isOccupied">是否被占用</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateLocationStatusAsync( string locationId , bool isOccupied );

        /// <summary>
        /// 分配托盘到库位
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>分配结果</returns>
        Task<bool> AssignPalletToLocationAsync( string locationId , string palletId );

        /// <summary>
        /// 从库位移除托盘
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>移除结果</returns>
        Task<bool> RemovePalletFromLocationAsync( string locationId );

        /// <summary>
        /// 查找可用的库位
        /// </summary>
        /// <param name="locationType">库位类型</param>
        /// <returns>可用库位列表</returns>
        Task<IEnumerable<Location>> FindAvailableLocationsAsync( LocationType locationType );

        /// <summary>
        /// 获取库位详情
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>库位详情</returns>
        Task<Location> GetLocationDetailsAsync( string locationId );
    }
}
