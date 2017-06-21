'use strict';

let path = require('path');

module.exports = {
    path: path.join(process.cwd(), './wwwroot/dist'),
    publicPath: '/',
    filename: '[name].bundle.js'
};
