const paths = require('./paths');
const common = require('./webpack.common.js');
const { merge } = require('webpack-merge');

const pathRewriteApi = (path) => path.replace(/^\/api/g, '');

module.exports = merge(common, {
  // Set the mode to development or production
  mode: 'development',
  target: ['web', 'es5'],
  optimization: {
    minimize: false,
  },
  // Control how source maps are generated
  devtool: 'inline-source-map',

  // Spin up a server for quick development
  devServer: {
    static: paths.build,
    liveReload: false,
    historyApiFallback: true,
    port: 3004,
    open: true,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
        'GET, POST, PUT, DELETE, PATCH, OPTIONS',
      'Access-Control-Allow-Headers':
        'X-Requested-With, content-type, Authorization',
    },
    hot: true,
    proxy: [
      {
        path: '/api/auth',
        changeOrigin: true,
        target: 'http://localhost:5266',
        pathRewrite: { '/api/auth': '' },
      },
      {
        path: '/api/user',
        changeOrigin: true,
        target: 'http://localhost:5266',
        pathRewrite: { '/api/user': '' },
      },
      {
        path: '/api/friends',
        changeOrigin: true,
        target: 'http://localhost:5165',
        pathRewrite: { '/api/friends': '' },
      },
    ],
  },
  output: {
    publicPath: '/',
    clean: true,
  },
});
