'use strict';

let path = require('path');
let ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
  rules: [
    {
      test: /\.ts$/,
      use: ['awesome-typescript-loader', 'angular2-template-loader']
    },
    {
      test: /\.html$/,
      use: 'raw-loader'
    },
    {
      test: /\.css$/,
      include: path.resolve(process.cwd(), 'wwwroot', 'app'),
      loaders: ['to-string-loader', 'css-loader']
    },
    {
      test: /\.css$/,
      exclude: path.resolve(process.cwd(), 'wwwroot', 'app'),
      loader: ExtractTextPlugin.extract({
        fallback: 'style-loader',
        use: 'css-loader'
      })
    }
  ]
};
