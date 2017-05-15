'use strict';
let path = require('path');

module.exports = {
  entry: require('./webpack/entry.aot'),

  context: path.join(process.cwd(), 'wwwroot'),

  output: require('./webpack/output'),

  module: require('./webpack/module'),

  plugins: require('./webpack/plugins'),

  resolve: require('./webpack/resolve'),

  devServer: require('./webpack/dev-server'),

  stats: 'errors-only',

  devtool: 'source-map'
};
