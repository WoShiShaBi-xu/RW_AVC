<template>
    <div class="warp">
        <div class="header">
            <div>近七天产能</div>
        </div>
        <div class="main">
            <div ref="chartContainer" style="width:100%;height:100%"></div>
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
        height: 0%;
        width: 100%;
    }

    .main {
        height: 100%;
        width: 100%;
    }
}
</style>

<script lang="ts" setup>
import Charts from '@jiaminghi/charts';
import { ref, onMounted } from 'vue';
import { getWeekCapacity, getProductName } from '@/api/capacity'

const chartContainer = ref(null);

const option1 = {
    legend: {
        textStyle: {
            fontFamily: 'Arial',
            fontSize: 13,
            fill: '#fff',
        },
    },
    grid: {
        bottom: '20%', 
    },
    xAxis: {
        name: '天',
        data: ["07-01", "07-02", "07-03", "07-04", "07-05", "07-06", "07-07"],
        nameTextStyle: {
            fill: '#FFF',
            fontSize: 11
        },
        axisLabel: {
            style: {
                fill: "#FFF",
                fontSize: 11,
                rotate: 40,
            },
        },
    },
    yAxis: {
        name: '产量（个）',
        nameTextStyle: {
            fill: '#FFF',
            fontSize: 13
        },
        data: 'value',
        splitLine: {
            show: false
        },
        axisLabel: {
            style: {
                fill: "#FFF",
                fontSize: 14,
                rotate: 0,
            },
        },
        min: 0
    },
    series: [
        {
            name: '总产量',
            data: [],
            type: 'line',
            color: '#00ffff',
            label: {
                show: true,
                style: {
                    fontSize: 11,
                    fill: '#FFF',
                    rotate: 90,
                    width: 20,
                    height: 10
                },
            },
            gradient: {
                color: ['#00ffff']
            }
        }
    ]
}

const fetchData = async (myChart: any) => {
    let data = await getWeekCapacity();
    let name = await getProductName();


    if (data.data != null && name.data != null) {
    // 先对 data.data 按照日期进行升序排序
    data.data.sort((a: any, b: any) => {
        return new Date(a.date).getTime() - new Date(b.date).getTime();
    });
    let dateList: any[] = [];
    let totalList: number[] = [];

    for (const model of data.data) {
        dateList.push(model.date);
        let dayTotal = 0;
        // 计算当日所有产品的总产量
        for (const productName of name.data) {
            if (model.type[productName]) {
                dayTotal += model.type[productName].count;
            }
        }
        totalList.push(dayTotal);
    }
    option1.xAxis.data = dateList;
    (option1.series[0].data as number[]) = totalList;

    myChart.setOption(option1);
}

};

onMounted(async () => {
    const myChart = new Charts(chartContainer.value);
    await fetchData(myChart);
    setInterval(async() => {    
        await fetchData(myChart);
    }, 120000)
});
</script>
