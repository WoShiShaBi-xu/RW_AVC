using RW.Framework.EventBus;
using RW.Framework.Extensions;
using RW.VAC.Application.Events.EventData;
using RW.VAC.Infrastructure.Opc;

namespace RW.VAC.Dashboard.subscribe;

public class DeviceSubscribe(IEventBus eventBus)
{
    public void StatusChanged( TagChangedEventArgs e )
    {
        // 创建一个新的 DeviceStatusEventData 对象，将从事件参数中获取的数据封装到该对象中
        var data = new DeviceStatusEventData
        {
            DeviceCode = e.GroupCode! ,  // 设备编码，对应前端的 deviceCode
            StatusValue = e.Value ,      // 设备状态值，对应前端的 statusValue
            TagType = e.ItemCode! ,      // 标签类型，对应前端的 tagType，用于区分状态类型
           
        };
        // 如果 ItemCode 是节拍的标识符，例如 "BeatTag"
        // 如果是节拍数据，则处理节拍值
        if ( e.IsBeat )
        {
            data.Beat = Convert.ToDouble( e.Value );
        }

        // 触发异步事件，将封装好的 data 对象发布到事件总线，供前端或其他订阅者使用
        eventBus.TriggerAsync( data );
    }
}