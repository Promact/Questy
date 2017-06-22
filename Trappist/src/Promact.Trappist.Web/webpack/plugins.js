'use strict';

let webpack = require('webpack');
let CopyWebpackPlugin = require('copy-webpack-plugin');
let ExtractTextPlugin = require('extract-text-webpack-plugin');
let path = require('path');

module.exports = [
  new webpack.ProgressPlugin(),
  new webpack.ContextReplacementPlugin(
    // The (\\|\/) piece accounts for path separators in *nix and Windows
    /angular(\\|\/)core(\\|\/)@angular/,
    path.join(process.cwd(), 'wwwroot')
  ),
  new ExtractTextPlugin('style.bundle.css')
];
