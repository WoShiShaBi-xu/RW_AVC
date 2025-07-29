using Microsoft.Extensions.Logging;
using RW.VAC.Application.Contracts.AGV;
using RW.VAC.Application.Services.Locations;
using RW.VAC.Domain.Location;
using RW.VAC.Infrastructure.Opc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Hardwares.Opc
{
    /// <summary>
    /// AGV按钮事件处理器
    /// 负责处理各个工作站的AGV加载和卸载按钮按下事件
    /// </summary>
    public class AgvButtonEventHandler
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<AgvButtonEventHandler> _logger;
        private readonly IAgvService _agvService;

        public AgvButtonEventHandler(
            ILocationService locationService ,
            ILogger<AgvButtonEventHandler> logger ,
            IAgvService agvService )
        {
            _locationService = locationService;
            _logger = logger;
            _agvService = agvService;
        }

        /// <summary>
        /// 处理工作站1的AGV加载按钮按下事件
        /// </summary>
        /// <param name="e">标签变化事件参数，包含按钮状态变化信息</param>
        /// <remarks>
        /// 当工作站1的AGV加载按钮被按下时触发此方法
        /// 用于启动AGV在工作站1的加载作业流程
        /// </remarks>
        public async void OnStation1AgvLoadButtonPressed( TagChangedEventArgs e )
        {
            //try
            //{
            //    if (Convert.ToBoolean(e.Value) ==true)
            //    {
            //        _logger.LogInformation("工作站1 AGV上料按钮被按下");

            //        // 获取护箱备料区最早绑定的库位
            //        var locationName = await GetEarliestBoundLocationInCasePreparationArea();

            //        if (!string.IsNullOrEmpty(locationName))
            //        {
            //            _logger.LogInformation($"找到护箱备料区最早绑定的库位: {locationName}");
            //            if (locationName == "1号护箱备料位")
            //            {
            //                var test = await _agvService.SendLoadCommandAsync("cst_n2uEAcCr", "zaijvhao", "mubiaodian", 52, 220);
            //                var test1 = await _locationService.UpdateLocationStatusAsync("LOC014", null);
            //                var del = await _agvService.DeleteLoadAsync(52, "");//删除载具
            //            }
            //            if (locationName == "2号护箱备料位")
            //            {
            //                var test = await _agvService.SendLoadCommandAsync("cst_n2uEAcCr", "zaijvhao", "mubiaodian", 52, 220);
            //                var test1 = await _locationService.UpdateLocationStatusAsync("LOC015", null);
            //                var del = await _agvService.DeleteLoadAsync(52, "");//删除载具
            //            }
            //            if (locationName == "3号护箱备料位")
            //            {
            //                var test = await _agvService.SendLoadCommandAsync("cst_n2uEAcCr", "zaijvhao", "mubiaodian", 52, 220);
            //                var test1 = await _locationService.UpdateLocationStatusAsync("LOC016", null);
            //                var del = await _agvService.DeleteLoadAsync(52, "");//删除载具
            //            }
            //            if (locationName == "4号护箱备料位")
            //            {
            //                var test = await _agvService.SendLoadCommandAsync("cst_n2uEAcCr", "zaijvhao", "mubiaodian", 52, 220);
            //                var test1 = await _locationService.UpdateLocationStatusAsync("PBP2507159937", null);
            //                var del = await _agvService.DeleteLoadAsync(52, "");//删除载具
            //            }
            //        }
            //        else
            //        {
            //            _logger.LogWarning("护箱备料区没有可用的绑定库位");
            //            // TODO: 处理没有可用库位的情况
            //        }
            //    }
               
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError( ex , "处理工作站1 AGV上料按钮事件时发生错误" );
            //}
        }

        /// <summary>
        /// 处理工作站6的AGV卸载按钮按下事件
        /// </summary>
        /// <param name="e">标签变化事件参数，包含按钮状态变化信息</param>
        /// <remarks>
        /// 当工作站6的AGV卸载按钮被按下时触发此方法
        /// 用于启动AGV在工作站6的卸载作业流程
        /// </remarks>
        public async void OnStation6AgvUnloadButtonPressed( TagChangedEventArgs e )
        {
            if (Convert.ToBoolean(e.Value) == true)
            {
                try
                {
                    _logger.LogInformation("工作站6 AGV卸载按钮被按下");

                    var del = await _agvService.AddLoadAsync(new AddLoadRequest() { LoadAngle = "0", LoadCode = "61", LoadSpecificationCode = "Specification_2_6AVneYvX", StorageCode = "230" });//新增载具

                    var test = await _agvService.SendLoadCommandAsync("cst_n2uEAcCr", "zaijvhao", "mubiaodian", 61, 227);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "处理工作站6 AGV卸载按钮事件时发生错误");
                }
            }
               
        }

        /// <summary>
        /// 处理工作站7的AGV加载按钮按下事件
        /// </summary>
        /// <param name="e">标签变化事件参数，包含按钮状态变化信息</param>
        /// <remarks>
        /// 当工作站7的AGV加载按钮被按下时触发此方法
        /// 用于启动AGV在工作站7的加载作业流程
        /// </remarks>
        public async void OnStation7AgvLoadButtonPressed( TagChangedEventArgs e )
        {
            //var get=_agvService.GetLoadsAsync();//查询所有载具位置

            //try
            //{
            //    _logger.LogInformation( "工作站7 AGV上料按钮被按下" );

            //    // 获取护箱备料区最早绑定的库位
            //    var locationName = await GetEarliestBoundLocationInCasePreparationArea();

            //    if (!string.IsNullOrEmpty( locationName ))
            //    {
            //        _logger.LogInformation( $"找到护箱备料区最早绑定的库位: {locationName}" );
            //        if (locationName == "1号护箱备料位")
            //        {
            //            var test = await _agvService.SendLoadCommandAsync( "cst_n2uEAcCr" , "zaijvhao" , "mubiaodian" , 52 , 220 );
            //            var test1 = await _locationService.UpdateLocationStatusAsync( "LOC014" , null );
            //            //var del = await _agvService.DeleteLoadAsync( 52 , "" );//删除载具
            //        }
            //        if (locationName == "2号护箱备料位")
            //        {
            //            var test = await _agvService.SendLoadCommandAsync( "cst_n2uEAcCr" , "zaijvhao" , "mubiaodian" , 52 , 220 );
            //            var test1 = await _locationService.UpdateLocationStatusAsync( "LOC015" , null );
            //           //var del = await _agvService.DeleteLoadAsync( 52 , "" );//删除载具
            //        }
            //        if (locationName == "3号护箱备料位")
            //        {
            //            var test = await _agvService.SendLoadCommandAsync( "cst_n2uEAcCr" , "zaijvhao" , "mubiaodian" , 52 , 220 );
            //            var test1 = await _locationService.UpdateLocationStatusAsync( "LOC016" , null );
            //            //var del = await _agvService.DeleteLoadAsync( 52 , "" );//删除载具
            //        }
            //        if (locationName == "4号护箱备料位")
            //        {
            //            var test = await _agvService.SendLoadCommandAsync( "cst_n2uEAcCr" , "zaijvhao" , "mubiaodian" , 52 , 220 );
            //            var test1 = await _locationService.UpdateLocationStatusAsync( "PBP2507159937" , null );
            //            //var del = await _agvService.DeleteLoadAsync( 52 , "" );//删除载具
            //        }
            //    }
            //    else
            //    {
            //        _logger.LogWarning( "护箱备料区没有可用的绑定库位" );
            //        // TODO: 处理没有可用库位的情况
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError( ex , "处理工作站1 AGV上料按钮事件时发生错误" );
            //}
        }

        /// <summary>
        /// 处理工作站8的AGV卸载按钮按下事件
        /// </summary>
        /// <param name="e">标签变化事件参数，包含按钮状态变化信息</param>
        /// <remarks>
        /// 当工作站8的AGV卸载按钮被按下时触发此方法
        /// 用于启动AGV在工作站8的卸载作业流程
        /// </remarks>
        public async void OnStation8AgvUnloadButtonPressed( TagChangedEventArgs e )
        {
            if (Convert.ToBoolean( e.Value ) == true)
            {
                try
                {
                    _logger.LogInformation( "工作站8 AGV卸载按钮被按下" );

                    var del = await _agvService.AddLoadAsync( new AddLoadRequest() { LoadAngle = "0" , LoadCode = "79" , LoadSpecificationCode = "Specification_2_6AVneYvX" , StorageCode = "236" } );//新增载具

                    var test = await _agvService.SendLoadCommandAsync( "cst_n2uEAcCr" , "zaijvhao" , "mubiaodian" , 79, 273 );

                }
                catch (Exception ex)
                {
                    _logger.LogError( ex , "处理工作站6 AGV卸载按钮事件时发生错误" );
                }
            }
        }

        /// <summary>
        /// 获取护箱备料区中最早绑定的库位名称
        /// </summary>
        /// <returns>最早绑定的库位名称，如果没有则返回null</returns>
        private async Task<string> GetEarliestBoundLocationInCasePreparationArea( )
        {
            try
            {
                // 获取所有护箱备料区的库位
                var locations = await _locationService.GetAllLocationsAsync();

                // 筛选护箱备料区且有绑定的库位
                var boundLocations = locations
                    .Where( loc => loc.LocationType == LocationType.护箱备料区 &&
                                  loc.CurrentBindingId.HasValue &&
                                  loc.CurrentBinding != null )
                    .ToList();

                if (!boundLocations.Any())
                {
                    _logger.LogInformation( "护箱备料区没有已绑定的库位" );
                    return null;
                }

                // 根据绑定时间排序，获取最早绑定的库位
                var earliestLocation = boundLocations
                    .OrderBy( loc => loc.CurrentBinding.BindTime ?? DateTime.MaxValue )
                    .FirstOrDefault();

                if (earliestLocation != null)
                {
                    _logger.LogInformation( $"护箱备料区最早绑定的库位: {earliestLocation.LocationName}, " +
                                         $"绑定时间: {earliestLocation.CurrentBinding.BindTime}, " +
                                         $"托盘ID: {earliestLocation.CurrentBinding.PalletId}, " +
                                         $"产品ID: {earliestLocation.CurrentBinding.ProductId}" );

                    return earliestLocation.LocationName;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex , "查询护箱备料区最早绑定库位时发生错误" );
                return null;
            }
        }
    }
}