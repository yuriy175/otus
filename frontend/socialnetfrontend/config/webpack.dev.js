const { merge } = require('webpack-merge')
const commonDev = require('./webpack.dev.common.js')
const ReactRefreshWebpackPlugin = require('@pmmmwh/react-refresh-webpack-plugin')
// const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin
const deps = require('../package.json').dependencies;
const ReactRefreshTypeScript = require('react-refresh-typescript');
const ModuleFederationPlugin = require('webpack/lib/container/ModuleFederationPlugin');

module.exports = merge(commonDev, {
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        exclude: /node_modules/,
        use: [
          {
            loader: 'ts-loader',
            options: {
              getCustomTransformers: () => ({
                before: [ReactRefreshTypeScript()],
              }),
            },
          },
        ],
      },
    ],
  },

  plugins: [
    // Only update what has changed on hot reload
    new ReactRefreshWebpackPlugin({ overlay: false}),
    // new BundleAnalyzerPlugin(),
  ],
})
