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
        path: '/cs/users',
        changeOrigin: true,
        target: 'http://localhost:5297',
        pathRewrite: { '/cs/users': '' },
      },
      {
        path: '/go/users',
        changeOrigin: true,
        target: 'http://localhost:55297',
        pathRewrite: { '/go/users': '' },
      },
      {
        path: '/cs/friends',
        changeOrigin: true,
        target: 'http://localhost:5297',
        pathRewrite: { '/cs/friends': '' },
      },
      {
        path: '/go/friends',
        changeOrigin: true,
        target: 'http://localhost:55297',
        pathRewrite: { '/go/friends': '' },
      },
      {
        path: '/cs/posts',
        changeOrigin: true,
        target: 'http://localhost:5297',
        pathRewrite: { '/cs/posts': '' },
      },
      {
        path: '/go/posts',
        changeOrigin: true,
        target: 'http://localhost:55297',
        pathRewrite: { '/go/posts': '' },
      },
    ],
  },
  output: {
    publicPath: '/',
    clean: true,
  },
});
