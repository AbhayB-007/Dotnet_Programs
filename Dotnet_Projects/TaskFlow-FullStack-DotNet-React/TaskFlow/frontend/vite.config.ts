import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    outDir: '../backend/src/TaskFlow.API/wwwroot',
    emptyOutDir: true,
  },
  server: {
    // https: true,
  }
})
