using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Dashboard.subscribe;
using RW.VAC.Infrastructure.Opc;
using RW.Framework.Extensions;
using Microsoft.AspNetCore.Http;
namespace RW.VAC.Dashboard.API.WebSocketManagers
{
    public class OpcNodeManager
    {
        private readonly IUaClient _uaClient;
        private readonly TagStorage _tag;
        private readonly IOpcItemService _opcItemService;
        private readonly IOpcGroupService _opcGroupService;
        private readonly DeviceSubscribe _deviceSubscribe;
        private readonly WebSocketManagers _webSocketManager;

        public OpcNodeManager( IUaClient uaClient , TagStorage tag , IOpcItemService opcItemService ,
                          IOpcGroupService opcGroupService , DeviceSubscribe deviceSubscribe, WebSocketManagers webSocketManager )
        {
            _uaClient = uaClient;
            _tag = tag;
            _opcItemService = opcItemService;
            _opcGroupService = opcGroupService;
            _deviceSubscribe = deviceSubscribe;
            _webSocketManager = webSocketManager;
        }
        public async Task<List<string>> LoadOpcNodeAsync( )
        {
            var groupList = await _opcGroupService.GetListAsync();
            var itemList = await _opcItemService.GetListAsync();
            var statusList = new List<string>();

            foreach ( var group in groupList )
            {
                var statusNode = itemList.Where( t =>
                    t.GroupId == group.Id &&
                    t.Code is TagTypeConsts.StatusTag
                        or TagTypeConsts.LackTag
                        or TagTypeConsts.OverLimitTag
                        or TagTypeConsts.BeatTag ).ToList();

                var prefix = group.Group.NotNullOrWhiteSpace()
                    ? string.Concat( group.Device , "." , group.Group )
                    : group.Device;

                statusNode.ForEach( t =>
                {
                    var nodeName = string.Concat( prefix , "." , t.Name );
                    _tag.AddTag( group.Code , t.Code , nodeName , _uaClient );
                    statusList.Add( nodeName );
                } );
            }

            // 点位订阅并绑定状态变化的回调方法
            _uaClient.Subscribe( statusList , "statusNode" , false , _deviceSubscribe.StatusChanged );

            return statusList;
        }
    }
}
