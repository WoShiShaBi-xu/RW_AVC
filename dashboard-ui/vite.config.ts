import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
import { resolve } from "path";
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      "@": resolve(__dirname, "./src"),
    },
  },
  server: {
    host: "127.0.0.1", // 强制使用 IPv4
    port: 3000,        // 指定端口为 3000（或其他未被占用的端口）
    strictPort: true,  // 如果端口被占用，则抛出错误，而不是尝试下一个端口
  },
})

