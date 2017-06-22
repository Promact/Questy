'use strict';

let webpack = require('webpack');
let CopyWebpackPlugin = require('copy-webpack-plugin');
let ExtractTextPlugin = require('extract-text-webpack-plugin');
let path = require('path');
let ngTool = require('@ngtools/webpack');

module.exports = [
    new webpack.ProgressPlugin(),
    new webpack.ContextReplacementPlugin(
        // The (\\|\/) piece accounts for path separators in *nix and Windows
        /angular(\\|\/)core(\\|\/)@angular/,
        path.join(process.cwd(), 'wwwroot')
    ),
    new ngTool.AotPlugin({
        tsConfigPath: './tsconfig-aot.json'
    }),
    new ExtractTextPlugin('style.bundle.css')
];
