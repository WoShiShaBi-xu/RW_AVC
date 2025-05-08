<template>
    <div class="warp">
        <div class="header">
            <div>产量（天）</div>
        </div>
        <div class="main">
            <div class="main-top">
                <div class="top-left">
                    <div class="main-tilte">产量</div>
                    <dv-digital-flop :config="capacityDateConfig" class="main-content" />
                </div>
                <!-- <div class="top-right">
                    <div class="main-tilte">异常数量</div>
                    <dv-digital-flop :config="offlineDateConfig" class="main-content" />
                </div> -->
            </div>
            <div class="header">
                <div>产量(小时)</div>
            </div>
            <div class="main-bottom">
                <div class="bottom-left">
                    <div class="main-tilte">产量</div>
                    <dv-digital-flop :config="capacityHourConfig" class="main-content" />
                </div>
                <!-- <div class="bottom-right">
                    <div class="main-tilte">异常数量</div>

                    <dv-digital-flop :config="offlineHourConfig" class="main-content" />

                </div> -->
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.warp {
    height: 100%;
    width: 100%;
    padding: 20px;

    .header {
        display: flex;
        justify-content: center;
        font-size: 25px;
        height: 10%;
        width: 100%;
    }

    .main {
        height: 90%;
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
        justify-content: flex-start; //纵轴对齐方式，默认是纵轴
        font-size: 18px;

        .main-top {
            width: 100%;
            height: 40%;
            display: flex;
            align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
            justify-content: flex-center; //纵轴对齐方式，默认是纵轴

            .top-left {
                width: 80%;
                height: 100%;
                display: flex;
                align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
                justify-content: flex-end; //纵轴对齐方式，默认是纵轴

                .main-tilte {
                    width: 30%;
                }
            }

            .top-right {
                width: 0%;
                height: 0%;
                display: flex;
                align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
                justify-content: flex-start; //纵轴对齐方式，默认是纵轴

                .main-tilte {
                    width: 0%;
                }
            }
        }

        .main-bottom {
            width: 100%;
            height: 50%;
            display: flex;
            align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
            justify-content: flex-center; //纵轴对齐方式，默认是纵轴

            .bottom-left {
                width: 80%;
                height: 100%;
                display: flex;
                align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
                justify-content: flex-end; //纵轴对齐方式，默认是纵轴

                .main-tilte {
                    width: 30%;
                }
            }

            .bottom-right {
                width: 0%;
                height: 0%;
                display: flex;
                align-items: center; // 纵轴对齐方式，默认是纵轴 子元素垂直居中
                justify-content: flex-start; //纵轴对齐方式，默认是纵轴

                .main-tilte {
                    width: 0%;
                }


            }

        }



    }
}
</style>

<script lang="ts" setup>
import { reactive, onMounted } from "vue";
import { getDateCapacity, getDateOffLine } from '@/api/capacity'
const offlineDateConfig = reactive({
    number: [100],
    style: { fill: 'red' },
    content: '{nt}个'

});
const capacityDateConfig = reactive({
    number: [100],
    style: { fill: '#00d9ff' },
    content: '{nt}个'

});

const offlineHourConfig = reactive({
    number: [100],
    style: { fill: 'red' },
    content: '{nt}个'

});
const capacityHourConfig = reactive({
    number: [100],
    style: { fill: '#00d9ff' },
    content: '{nt}个'

});

const fetchData = async () => {
    let capacityDate = await getDateCapacity(1);
    let offlineDate = await getDateOffLine(1);
    let capacityHour = await getDateCapacity(2);
    let offlineHour = await getDateOffLine(2);
    capacityDateConfig.number = [capacityDate.data];
    offlineDateConfig.number = [offlineDate.data];
    capacityHourConfig.number = [capacityHour.data];
    offlineHourConfig.number = [offlineHour.data];
}

onMounted(async () => {

    await fetchData();

    setInterval(async () => {
        await fetchData();
    }, 3000)
});
</script>