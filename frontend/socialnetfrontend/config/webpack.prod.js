const { merge } = require('webpack-merge')

const paths = require('./paths')
const common = require('./webpack.common.js')
const ModuleFederationPlugin = require('webpack/lib/container/ModuleFederationPlugin');
const deps = require('../package.json').dependencies;
module.exports = merge(common, {
  mode: 'production',
  devtool: false,

  // Control how source maps are generated
  module: {
    rules:  [
      {
        test: /\.[t]sx?$/, exclude: /node_modules/,
        use: [{
          loader: require.resolve('esbuild-loader'),
          options: {
            loader: 'tsx',
            target: 'es2016'
          }
        }]
      }
    ],
  },
  output: {
    uniqueName: 'mfCore',
    filename: '[name].bundle.js',
    publicPath: '/',
  },
  // performance: {
  //   hints: false,
  //   maxEntrypointSize: 512000,
  //   maxAssetSize: 512000,
  // },
  plugins: [
    // Only update what has changed on hot reload
    // new webpack.HotModuleReplacementPlugin(),
    new ModuleFederationPlugin(
      {
        name: 'coreMf',
        filename:
          'remoteEntryCoreMf.js',
        remotes: {
          auth: "auth@/auth/remoteEntryMF1.js",
          importer: "importer@/importer/remoteEntryImporterMf.js",
          window2D: "window2D@/window2D/remoteEntryWindow2DMf.js",
          window3D: "window3D@/window3D/remoteEntryWindow3DMf.js"
        },
        shared: {
          react: {
            singleton: true,
            requiredVersion: deps.react,
          },
          "react-dom": {
            singleton: true,
            requiredVersion: deps["react-dom"],
          },
          'react-router-dom': {
            singleton: true,
            version: deps['react-router-dom']
          },
          'ag-grid-enterprise': {
            singleton: true,
            version: deps['ag-grid-enterprise']
          },
          'ag-grid-react': {
            singleton: true,
            version: deps['ag-grid-react']
          },
          'ag-grid-community': {
            singleton: true,
            version: deps['ag-grid-community']
          },
        }
      }
    ),
    // new ReactRefreshWebpackPlugin(),
    // new BundleAnalyzerPlugin(),
  ],
})
