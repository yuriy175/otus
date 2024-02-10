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
      // {
      //   path: '/wsapp/go',
      //   target: 'ws://localhost:8005/',
      //   changeOrigin: true,
      //   pathRewrite: { '/wsapp/go': '' },
      //   ws: true,
      // },
      // {
      //   path: '/wsapp/cs',
      //   target: 'ws://localhost:8006/',
      //   changeOrigin: true,
      //   pathRewrite: { '/wsapp/cs': '' },
      //   ws: true,
      // },
      {
        path: '/wsapp',
        //target: 'ws://localhost:5230/',
        target: 'ws://localhost:55230/',
        changeOrigin: true,
        pathRewrite: { '/wsapp': '' },
        ws: true,
      },
      {
        path: '/api',
        changeOrigin: true,
        //target: 'http://localhost:5297',
        //target: 'http://localhost:55297',
        target: 'http://localhost:8009',
        //target: 'http://localhost:8010',
        pathRewrite: { '/api': '' },
      },
    ],
  },
  output: {
    publicPath: '/',
    clean: true,
  },
});
