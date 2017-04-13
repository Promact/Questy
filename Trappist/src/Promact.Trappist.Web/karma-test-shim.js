/*global jasmine, __karma__, window*/
Error.stackTraceLimit = Infinity;
jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000;


__karma__.loaded = function () {
};


function isJsFile(path) {
    return path.slice(-3) === '.js';
}

function isSpecFile(path) {
    return path.slice(-8) === '.spec.js';
}

function isBuiltFile(path) {
    var builtPath = '/base/wwwroot/app/';
    return isJsFile(path) && (path.substr(0, builtPath.length) === builtPath);
}

var allSpecFiles = Object.keys(window.__karma__.files)
  .filter(isSpecFile)
  .filter(isBuiltFile);

// Load our SystemJS configuration.
System.config({
    baseURL: '/base'
});

System.config({
    packages: {
        'app': {
            main: 'main.js',
            defaultExtension: 'js'
        },
        'rxjs': {
            defaultExtension: 'js'
        },
        
        'angular2-wizard': {
            main: 'index.js',
            defaultExtension: 'js'
        }
    },

    map: {
        'rxjs': 'node_modules/rxjs',
        '@angular': 'node_modules/@angular',
        'app': 'wwwroot/app',
        '@angular/material': 'node_modules/@angular/material/bundles/material.umd.js',
        '@angular/core': 'node_modules/@angular/core/bundles/core.umd.js',
        '@angular/common': 'node_modules/@angular/common/bundles/common.umd.js',
        '@angular/compiler': 'node_modules/@angular/compiler/bundles/compiler.umd.js',
        '@angular/platform-browser': 'node_modules/@angular/platform-browser/bundles/platform-browser.umd.js',
        '@angular/platform-browser-dynamic': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
        '@angular/http': 'node_modules/@angular/http/bundles/http.umd.js',
        '@angular/router': 'node_modules/@angular/router/bundles/router.umd.js',
        '@angular/forms': 'node_modules/@angular/forms/bundles/forms.umd.js',
        '@angular/core/testing': 'node_modules/@angular/core/bundles/core-testing.umd.js',
        '@angular/platform-browser-dynamic/testing': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic-testing.umd.js',
        '@angular/compiler/testing': 'node_modules/@angular/compiler/bundles/compiler-testing.umd.js',
        '@angular/platform-browser/testing': 'node_modules/@angular/platform-browser/bundles/platform-browser-testing.umd.js',
        '@angular/common/testing': 'node_modules/@angular/common/bundles/common-testing.umd.js',
        '@angular/http/testing': 'node_modules/@angular/http/bundles/http-testing.umd.js',
        '@angular/router/testing': 'node_modules/@angular/router/bundles/router-testing.umd.js',
        '@angular/forms/testing': 'node_modules/@angular/forms/bundles/forms-testing.umd.js',
        'angular2-wizard': 'node_modules/angular2-wizard/dist/',
        
    }
});


Promise.all([
 System.import('@angular/core/testing'),
 System.import('@angular/platform-browser-dynamic/testing')
]).then(function (providers) {
    var testing = providers[0];
    var testingBrowser = providers[1];

    testing.TestBed.initTestEnvironment(testingBrowser.BrowserDynamicTestingModule,
      testingBrowser.platformBrowserDynamicTesting());

}).then(function () {
    // Finally, load all spec files.
    // This will run the tests directly.
    return Promise.all(
      allSpecFiles.map(function (moduleName) {
          return System.import(moduleName);
      }));
}).then(__karma__.start, __karma__.error);
