<template>
    <div class="warp">
        <div class="header">
            <div>工艺参数</div>
        </div>
        <div class="main">
            <!-- 表格渲染 -->
            <div class="table-header">
                <div style="margin-left:49%;">|开关测试质量| </div>
                <div style="margin-left:18%;" >|液压测试质量| </div>
            </div>
            <dv-scroll-board :config="config" style="width:100%; height:95%; margin-top:-1%;" />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { onMounted, reactive } from "vue";
import { getProcessDate } from '@/api/capacity'

const config = reactive<{
    header: string[],
    data: string[][],
    index: boolean,
    columnWidth: number[],
    align: string[],
    indexHeader: string,
}>({
    header: ['SN', '气密性测试' ,'扭矩值设定/Nm', '实时扭矩值/Nm', '泄露量设定值min/mL', '实时泄漏量min/mL'],
    data: [], // 初始化为 string[][] 类型
    index: true,
    columnWidth: [60, 250, 180, 150, 170, 150, 150],
    align: ['center','center','center','center','center','center','center','center'],
    indexHeader: "序号",
});

const fetchData = async () => {
    try {
        let processDate = await getProcessDate();
        console.log("processDate", processDate.data);

        // 转换并处理数据格式
        const transformedData = processDate.data.map((item: any) => {
            // 获取原始值，防止空值
            const sn = item.sn || '';

            // 处理 airtightpracticeValue 显示及颜色
            let airtightpractice = '';
            if (item.airtightpracticeValue === '1') {
                // 1为合格（绿色）
                airtightpractice = '<span style="color: green;">合格</span>';
            } else if (item.airtightpracticeValue === '2') {
                // 2为不合格（红色）
                airtightpractice = '<span style="color: red;">不合格</span>';
            } else {
                airtightpractice = '';
            }

            // 将值转为字符串并尝试转换为数字用于比较
            const swithValue = item.swithValue?.toString() || '';
            const swithpracticeValue = item.swithpracticeValue?.toString() || '';
            const hydraulicValue = item.hydraulicValue?.toString() || '';
            const hydraulicpracticeValue = item.hydraulicpracticeValue?.toString() || '';

            // 数值比较（若无法转成数字则默认为0，以防比较异常）
            const swithValueNum = parseFloat(swithValue) || 0;
            const swithpracticeNum = parseFloat(swithpracticeValue) || 0;

            const hydraulicValueNum = parseFloat(hydraulicValue) || 0;
            const hydraulicpracticeNum = parseFloat(hydraulicpracticeValue) || 0;

            // 如果swithpracticeValue > swithValue则显示为红色
            let swithpracticeDisplay = swithpracticeValue;
            if (swithpracticeNum > swithValueNum) {
                swithpracticeDisplay = `<span style="color:red;">${swithpracticeValue}</span>`;
            }

            // 如果hydraulicpracticeValue > hydraulicValue则显示为红色
            let hydraulicpracticeDisplay = hydraulicpracticeValue;
            if (hydraulicpracticeNum > hydraulicValueNum) {
                hydraulicpracticeDisplay = `<span style="color:red;">${hydraulicpracticeValue}</span>`;
            }

            // 返回最终数据（如果前端表格需要直接显示HTML，请确保表格支持HTML渲染）
            return [
                sn,
                airtightpractice,            // 已根据值确定文本和颜色
                swithValue,
                swithpracticeDisplay,        // 如果超标则红色
                hydraulicValue,
                hydraulicpracticeDisplay,    // 如果超标则红色
            ];
        });

        // 更新 config.data
        config.data = transformedData;

    } catch (error) {
        console.error("Error fetching data:", error);
    }
};



onMounted(async () => {
    console.log("工艺参数加载函数触发");
    await fetchData();
    setInterval(async () => {
        await fetchData();
    }, 120000);  // 两分钟刷新一次
});

// 初始化表格数据并动态处理样式
const initData = () => {
    const rawData = [
        ['FM1324567891','合格', '231', '12', '32', '23', '12'],
        ['FM1324567892', '合格','8045', '232', '12', '22', '23', '12'],
        ['FM1324567893','合格' ,'231', '231', '12', '12', '23', '23'],
        ['FM1324567894','合格' ,'234', '300', '12', '1', '23', '45'],
        ['FM1324567895','合格' ,'231', '232', '12', '23', '23', '12'],
        ['FM1324567896','合格' ,'874', '343', '12', '12', '23', '12'],
        ['FM1324567897','合格' ,'454', '343', '12', '342', '23', '34'],
        ['FM1324567898','合格' ,'454', '343', '12', '12', '23', '12'],
    ];

    // 对原始数据进行处理，添加样式信息
    config.data = rawData.map((row) =>
        row.map((cell, index) => {
            // 检查实时值列是否超出设定值
            if (index === 2 || index === 4 || index === 6) {
                const setValue = parseFloat(row[index - 1]);
                const realValue = parseFloat(cell);
                if (realValue > setValue) {
                    return `<span style="color: red;">${cell}</span>`;
                }
            }
            return cell;
        })
    );
};

// 初始化数据
initData();
</script>

<style lang="scss" scoped>
.warp {
    height: 100%;
    width: 100%;
    padding: 20px;

    .header {
        display: flex;
        justify-content: center;
        font-size: 25px;
        height: 7%;
        width: 100%;
    }

    .main {
        height: 90%;
        width: 100%;
        .table-header {
            display: flex;
            text-align: center;
            font-weight: bold;
            background-color:#00baff;
            padding: 10px;
            margin-bottom: 10px;
        }
    }
}

</style>
