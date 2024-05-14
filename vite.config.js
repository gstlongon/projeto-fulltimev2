import { fileURLToPath, URL } from 'node:url'
import { createProxyMiddleware } from 'http-proxy-middleware';

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})

export const devServer = {
  before: function (app) {
      app.use(
          '/api',
          createProxyMiddleware({
              target: 'https://api.postmon.com.br',
              changeOrigin: true,
              pathRewrite: {
                  '^/api': '', // opcional, se o prefixo da API for diferente
              },
          })
      );
  },
};
