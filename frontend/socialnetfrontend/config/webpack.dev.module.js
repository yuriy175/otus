const { merge } = require('webpack-merge')
const commonDev = require('./webpack.dev.common.js')
const paths = require('./paths')
const ReactRefreshWebpackPlugin = require('@pmmmwh/react-refresh-webpack-plugin')
const ExternalTemplateRemotesPlugin = require('external-remotes-plugin')
const LiveReloadPlugin = require('webpack-livereload-plugin')
// const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin
const deps = require('../package.json').dependencies;
const ReactRefreshTypeScript = require('react-refresh-typescript');
module.exports = merge(commonDev, {
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: 'babel-loader',
                exclude: /node_modules/,
                options: {
                    presets: ['@babel/preset-react',  '@babel/preset-typescript']
                },
            },
        ],
    },

    plugins: [
        new ExternalTemplateRemotesPlugin(),
        new LiveReloadPlugin({
            port: 35729
        }),
        new ReactRefreshWebpackPlugin({ overlay: false}),
        // new BundleAnalyzerPlugin(),
    ],
})
