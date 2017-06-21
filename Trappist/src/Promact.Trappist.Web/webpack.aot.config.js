'use strict';
let path = require('path');

module.exports = {
  entry: require('./webpack/entry.aot'),

  context: path.join(process.cwd(), 'wwwroot'),

  output: require('./webpack/output'),

  module: require('./webpack/module.prod'),

  plugins: require('./webpack/plugin.prod'),

  resolve: require('./webpack/resolve'),

  stats: 'errors-only',

  devtool: 'source-map'
};
