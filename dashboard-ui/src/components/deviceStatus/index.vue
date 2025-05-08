<template>
    <div class="warp">
       
            <div class="header">
            
            <dv-decoration3 />
            <div>设备状态</div>
            <dv-decoration3 />
            
        </div>
        <div style="text-align: right;">
            <status :statusValue='"info"' />
            未连接总控
                    <status :statusValue='"warning"' />
                   未连接PLC
                    <status :statusValue='"primary"' />
             待机
                    <status :statusValue='"success"' />
            运行中
                    <status :statusValue='"danger"' />
            设备故障
        </div>
        <LoadingBar width="100%" height="6px" animationDuration="3s" />
        <div class="main">
            <div class="main-top">
                <!-- 自动上料桁架系统 -->
                <div style="width: 12%;">
                    <div class="statusNameValue" style="margin-left: -10%; margin-top: 10%;">
                        <div class="stausName"> 自动上料桁架系统</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M01.beat }}
                            </div> -->
                        <div class="statusValue"style=" margin-top: -10%;">

                            <div>
                                <status :statusValue="DataStatus.M01.statusValue" />
                                {{ DataStatus.M01.statusName }}
                            </div>
                            <div v-if="DataStatus.M01.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M01.overLimitValue" />
                                {{ DataStatus.M01.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M01.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M01.lackNodeValue" />
                                {{ DataStatus.M01.lackNodeName }}
                            </div>
                           
                        </div>
                    </div>
                    <arrow style=" width: 10%; margin-left: 30%; transform: rotate(90deg); margin-top:0%;" />
                </div>
                
                <!-- 叶片推杆紧固专机 -->
                <div style="width: 12%; margin-left: 9.5%; ">
                    <div class="statusNameValue" style="margin-left: 20%;margin-top: -20%;">
                        <div class="stausName"> 叶片推杆紧固专机</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M06.beat }}
                            </div> -->
                        <div class="statusValue"style="margin-top: -10%;">
                            <div>
                                <status :statusValue="DataStatus.M06.statusValue" />
                                {{ DataStatus.M06.statusName }}
                            </div>
                            <div v-if="DataStatus.M06.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M06.overLimitValue" />
                                {{ DataStatus.M06.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M06.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M06.lackNodeValue" />
                                {{ DataStatus.M06.lackNodeName }}
                            </div>
                           
                        </div>
                    </div>
                    <arrow style=" width: 50%; margin-left: 50%; transform: rotate(90deg); margin-top:15%;" />
                </div>

                <!-- 压圈螺母组装机-->
                <div style="width: 12%; margin-left: -2%;">
                    <div class="statusNameValue" style="margin-left: 20%;margin-top:8%;">
                        <div class="stausName">压圈螺母组装机</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M08.beat }}
                            </div> -->
                        <div class="statusValue"style="margin-left:-5%; margin-top: -10%;">

                            <div>
                                <status :statusValue="DataStatus.M08.statusValue" />
                                {{ DataStatus.M08.statusName }}
                            </div>
                            <div v-if="DataStatus.M08.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M08.overLimitValue" />
                                {{ DataStatus.M08.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M08.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M08.lackNodeValue" />
                                {{ DataStatus.M08.lackNodeName }}
                            </div>

                        </div>

                    </div>
                    <arrow style=" width: 20%; margin-left: 56%; transform: rotate(90deg); margin-top: 0%;" />
                </div>
                <!-- 开关测试台 -->
                <div style="width: 12%;margin-left: 0.5%;">

                    <div class="statusNameValue" style="margin-left: 40%;margin-top: 7%;">
                        <div class="stausName">开关测试</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M10.beat }}
                            </div> -->
                        <div class="statusValue"style=" margin-top: -10%;">
                            <div>
                                <status :statusValue="DataStatus.M10.statusValue" />
                                {{ DataStatus.M10.statusName }}
                            </div>
                            <div v-if="DataStatus.M10.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M10.overLimitValue" />
                                {{ DataStatus.M10.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M10.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M10.lackNodeValue" />
                                {{ DataStatus.M10.lackNodeName }}
                            </div>
                            
                        </div>
                    </div>
                    <arrow style=" width: 20%; margin-left: 80%; transform: rotate(90deg); margin-top: 0%;" />
                </div>
                <!--液压试验台 -->
                <div style="width: 12%;margin-left: 2%;">

                    <div class="statusNameValue" style="margin-left: 75%; margin-top: 7%;">
                        <div class="stausName"> 液压测试</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M12.beat }}
                            </div> -->
                        <div class="statusValue" style=" margin-top: -10%;">
                            <div>
                                <status :statusValue="DataStatus.M12.statusValue" />
                                {{ DataStatus.M12.statusName }}
                            </div>
                            <div v-if="DataStatus.M12.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M12.overLimitValue" />
                                {{ DataStatus.M12.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M12.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M12.lackNodeValue" /> 
                                {{ DataStatus.M12.lackNodeName }}
                            </div>
                            
                        </div>
                    </div>
                    <arrow style=" width: 50%; margin-left: 100%; transform: rotate(90deg); margin-top: 0%;" />
                </div>
                 <div style="width: 5%;">
                   
                    <div class="statusNameValue" style="margin-left: 230%; margin-top: 100%;">
                        <div class="stausName">人工位3</div>
                    </div>
                     <arrow style=" margin-top:0%; margin-left: 230%; width: 100%;" />
                </div>
                <div style="width: 12%;margin-left: 9%;">

                    <div class="statusNameValue" style="margin-left: 29%;margin-top:7%;">
                        <div class="stausName"> 阀盖自动组装专机</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M15.beat }}
                            </div> -->
                        <div class="statusValue"style=" margin-top:-10%;">
                            <div>
                                <status :statusValue="DataStatus.M15.statusValue" />
                                {{ DataStatus.M15.statusName }}
                            </div>
                            <div v-if="DataStatus.M15.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M15.overLimitValue" />
                                {{ DataStatus.M15.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M15.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M15.lackNodeValue" />
                                {{ DataStatus.M15.lackNodeName }}
                            </div>
                            
                        </div>
                    </div>
                    <arrow style=" width: 20%; margin-left: 70%; transform: rotate(90deg); margin-top: 10%;" />
                </div>
                 <div style="width: 5%;">
                   
                    <div class="statusNameValue" style="margin-left: 30%; margin-top: 100%;">
                        <div class="stausName">人工位4</div>
                    </div>
                     <arrow style=" margin-top:10%; margin-left: 60%; width: 40%;" />
                </div>
            </div>
            <div class="main-center">
                <div class="equipment"></div>
            </div>
            <div class="main-bottom">

                <div style="width: 12%;">
                   
                    <div class="statusNameValue" style="margin-left: 50%; margin-top: -165%;">
                        <div class="stausName">钻孔专机</div>
                       
                        <div class="statusValue" style="margin-left: -5%;">
                            <div>
                                <status :statusValue="DataStatus.M02.statusValue" />
                                {{ DataStatus.M02.statusName }}
                            </div>
                            <div v-if="DataStatus.M02.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M10.overLimitValue" />
                                {{ DataStatus.M02.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M02.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M02.lackNodeValue" />
                                {{ DataStatus.M02.lackNodeName }}
                            </div>
                            
                        </div>
                    </div>
                     <arrow style=" margin-top:15%; margin-left: 80%; width: 30%;" />
                </div>
                 <div style="width: 5%;">
                   
                    <div class="statusNameValue" style="margin-left: -10%; margin-top: -250%;">
                        <div class="stausName">人工位1</div>
                    </div>
                     <arrow style=" margin-top:50%; margin-left: -10%; width: 100%;" />
                </div>

                <div style="width: 12%;">
                   
                    <div class="statusNameValue" style="margin-left: -20%; margin-top: -130%;">
                        <div class="stausName">叶片推杆组装机</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M04.beat }}
                            </div> -->
                        <div class="statusValue" style="margin-left: -5%;">
                            <div>
                                <status :statusValue="DataStatus.M04.statusValue" />
                                {{ DataStatus.M04.statusName }}
                            </div>
                            <div v-if="DataStatus.M04.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M04.overLimitValue" />
                                {{ DataStatus.M04.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M04.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M04.lackNodeValue" />
                                {{ DataStatus.M04.lackNodeName }}
                            </div>
                            
                        </div>
                    </div>
                     <arrow style=" margin-top: 20%; margin-left:0%; width: 60%;" />
                </div>
                 <div style="width: 5%;">
                   
                    <div class="statusNameValue" style="margin-left: -120%; margin-top: -250%;">
                        <div class="stausName">人工位2</div>
                    </div>
                     <arrow style=" margin-top:20%; margin-left: -115%; width: 100%;" />
                </div>
                <div style="width: 12%; ">
                    
                    <div class="statusNameValue" style="margin-left: 65%; margin-top: -165%;">
                        <div class="stausName">气密性测试</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M09.beat }}
                            </div> -->
                        <div class="statusValue"style="margin-left: -3%;">
                            <div>
                                <status :statusValue="DataStatus.M09.statusValue" />
                                {{ DataStatus.M09.statusName }}
                            </div>
                            <div v-if="DataStatus.M09.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M09.overLimitValue" />
                                {{ DataStatus.M09.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M09.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M09.lackNodeValue" />
                                {{ DataStatus.M09.lackNodeName }}
                            </div>
                           
                        </div>
                    </div>
                    <arrow style=" margin-top: 18%; margin-left:90%; width: 40%;" />
                </div>
                <div style="width: 12%;">
                    
                    <div class="statusNameValue" style="margin-left: 80%; margin-top: -165%;">
                        <div class="stausName" style="width: 100%;"> 指示牌组装专机</div>
                        <!-- <div class="beat">
                                节拍: {{ DataStatus.M11.beat }}
                            </div> -->
                        <div class="statusValue">
                            <div>
                                <status :statusValue="DataStatus.M11.statusValue" />
                                {{ DataStatus.M11.statusName }}
                            </div>
                            <div v-if="DataStatus.M11.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M11.overLimitValue" />
                                {{ DataStatus.M11.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M11.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M11.lackNodeValue" />
                                {{ DataStatus.M11.lackNodeName }}
                            </div>
                            
                        </div>
                    </div>
                    <arrow style=" margin-top: 18%; margin-left: 100%; width: 50%;" />
                </div>

                <div style="width: 12%; margin-left: 26%; ">
                   
                    <div class="statusNameValue" style="margin-left: -60%; margin-top: -160%;">
                        <div class="stausName"> 自动贴标专机</div>
                       
                        <div class="statusValue">
                            <div>
                                <status :statusValue="DataStatus.M14.statusValue" />
                                {{ DataStatus.M14.statusName }}
                            </div>
                            <div v-if="DataStatus.M14.overLimitName ==='NG超限'">
                                <status :statusValue="DataStatus.M14.overLimitValue" />
                                {{ DataStatus.M14.overLimitName }}
                            </div>
                            <div v-if="DataStatus.M14.lackNodeName === '缺料'">
                                <status :statusValue="DataStatus.M14.lackNodeValue" />
                                {{ DataStatus.M14.lackNodeName }}
                            </div>

                        </div>

                    </div>
                     <arrow style=" margin-top: 25%; margin-left: -5%; width: 60%;" />
                </div>
            </div>
        </div>

    </div>
</template>

<script setup lang="ts">
import status from "../icon/status.vue";
import arrow from "../icon/arrow.vue";
import LoadingBar from '../icon/LoadingBar.vue';
import {  onMounted, reactive } from "vue";

interface ModuleStatus {  
    statusValue: string;  
    statusName: string;  
    overLimitValue: string;  
    overLimitName: string;  
    lackNodeValue: string;  
    lackNodeName: string;  
    beat: number;
}
const DataStatus: Record<string, ModuleStatus>  = reactive({
    M01: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M02: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M04: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M06: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M08: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M09: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M10: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M11: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M12: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M14: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
    M15: { statusValue: 'info', statusName: '未连接', overLimitValue: 'info', overLimitName: '未连接', lackNodeValue: 'info', lackNodeName: '未连接', beat: 0 },
});

onMounted(() => {
     const socket = new WebSocket('wss://localhost:7074/ws');
    //const socket = new WebSocket('ws://10.132.150.138:52024/ws');
    socket.onopen = () => {
        console.log('WebSocket 已连接');
    };

    socket.onmessage = (event) => {
        const statu = JSON.parse(event.data);
        console.log('收到数据'+statu);
        // 更新 DataStatus 数据
        if (statu.TagType === 'Status') {
            switch (statu.StatusValue) {
                case 0: DataStatus[statu.DeviceCode].statusValue = 'warning'; DataStatus[statu.DeviceCode].statusName = '未连接'; break;
                case 1: DataStatus[statu.DeviceCode].statusValue = 'primary'; DataStatus[statu.DeviceCode].statusName = '待机'; break;
                case 2: DataStatus[statu.DeviceCode].statusValue = 'success'; DataStatus[statu.DeviceCode].statusName = '生产中'; break;
                case 3: DataStatus[statu.DeviceCode].statusValue = 'danger'; DataStatus[statu.DeviceCode].statusName = '设备故障'; break;
            }
        }
        if (statu.TagType === 'Lack') {
            DataStatus[statu.DeviceCode].lackNodeValue = statu.StatusValue ? 'danger' : 'info';
            DataStatus[statu.DeviceCode].lackNodeName = statu.StatusValue ? '缺料' : '未缺料';
        }
        if (statu.TagType === 'NGOverLimit') {
            DataStatus[statu.DeviceCode].overLimitValue = statu.StatusValue ? 'warning' : 'info';
            DataStatus[statu.DeviceCode].overLimitName = statu.StatusValue ? 'NG超限' : '未NG超限';
        }
        if (statu.TagType === 'Beat') {
            DataStatus[statu.DeviceCode].beat = statu.StatusValue;
        }
    };

    socket.onclose = () => {
        console.log('WebSocket 已断开');
    };

    socket.onerror = (error) => {
        console.error('WebSocket 错误:', error);
    };
});



onMounted(() => {
    // console.log(temp['M02'])
});

</script>

<style lang="scss" scoped>
.warp {
    height: 100%;
    width: 100%;
    padding: 20px;

    .header {
        display: flex;
        justify-content: space-between;
        font-size: 25px;
        width: 100%;

        .dv-decoration-3 {
            height: 30px;
            width: 200px;
        }
    }

    .main {
        display: flex;
        margin-top: 50px;
        width: 100%;
        height: 200%;
        flex-direction: column;

        .main-center {
            height: 30%;
            width: 100%;

            .equipment {
                height: 70%;
                width: 100%;
                background-image: url("@/assets/device.png");
                background-size: 100% 100%;
            }
        }

        .main-bottom {
            width: 100%;
            height: 10%;
            display: flex;
        }

        .main-top {
            width: 100%;
            height: 15%;
            display: flex;
        }
    }
}

.statusNameValue {
    width: 100%;
    height: 100%;

    display: flex;
    flex-direction: column;
    //align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
    //justify-content: center; //纵轴对齐方式，默认是纵轴

}

.statusName {
    width: 50%;
    height: 100%;

}

.statusValue {
    width: 230px;
    height: 100%;

    display: flex;


    align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
    justify-content: center; //纵轴对齐方式，默认是纵轴
    margin-left: 0%;


}

.statusValue>div {

    text-align: center;

}
</style>