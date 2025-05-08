<template>
    <span class="sc-state" :class="[{ 'sc-status-processing': pulse }, 'sc-state-bg--' + props.statusValue]"></span>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";

const pulse = ref(true);



const props = defineProps({
    statusValue: { type: String, default: 'danger' },
});
onMounted(() => {


});
</script>

<style lang="scss" scoped>
.sc-state {
    display: inline-block;
    background: #000;
    width: 10px;
    height: 10px;
    border-radius: 50%;
    vertical-align: middle;
}

.sc-status-processing {
    position: relative;
}

.sc-status-processing:after {
    position: absolute;
    top: 0%;
    left: 0%;
    width: 100%;
    height: 100%;
    border-radius: 50%;
    background: inherit;
    content: '';
    animation: warn 1.2s ease-in-out infinite;
}

.sc-state-bg--primary {
    background: var(--el-color-primary);
}

.sc-state-bg--success {
    background: var(--el-color-success);
}

.sc-state-bg--warning {
    background: var(--el-color-warning);
}

.sc-state-bg--danger {
    background: var(--el-color-danger);
}

.sc-state-bg--info {
    background: var(--el-color-info);
}

@keyframes warn {
    0% {
        transform: scale(0.5);
        opacity: 1;
    }

    30% {
        opacity: 0.7;
    }

    100% {
        transform: scale(2.5);
        opacity: 0;
    }
}
</style>