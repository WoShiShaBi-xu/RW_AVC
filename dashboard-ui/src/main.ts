import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import dataV from '@kjgl77/datav-vue3';
import ElementPlus from 'element-plus';
import 'element-plus/dist/index.css'
import 'element-plus/theme-chalk/display.css'
const app = createApp(App);
app.use(dataV);
app.use(ElementPlus);
app.mount('#app')
window.LackNode = {};
window.StatusNode = {};
window.OverLimitNode = {};