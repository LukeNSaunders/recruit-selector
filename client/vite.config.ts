import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    // Send any /api request to the C# server
    proxy: {
      '/api': 'http://localhost:5000',
    },
  },
})
