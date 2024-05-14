import { createProxyMiddleware } from 'http-proxy-middleware';

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
  