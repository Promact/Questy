// Karma configuration
// Generated on Sat Aug 13 2016 15:45:03 GMT+0530 (India Standard Time)

module.exports = function (config) {
    config.set({

        // base path that will be used to resolve all patterns (eg. files, exclude)
        basePath: '',


        // frameworks to use
        // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
        frameworks: ['jasmine'],

        // list of files / patterns to load in the browser
        files: [// Polyfills.
                 
               //'node_modules/traceur/bin/traceur-runtime.js',
                //'node_modules/es6-shim/es6-shim.js',
                //'node_modules/reflect-metadata/Reflect.js',
                { pattern: 'node_modules/reflect-metadata/Reflect.js.map', included: false, watched: false },

                'node_modules/systemjs/dist/system-polyfills.js',
                 { pattern:  'node_modules/systemjs/dist/system-polyfills.js', included: false, watched: false },
                'node_modules/systemjs/dist/system.src.js',
                'node_modules/core-js/client/shim.js',
                'node_modules/zone.js/dist/zone.js',
                'node_modules/zone.js/dist/proxy.js',
                'node_modules/zone.js/dist/sync-test.js',
                'node_modules/zone.js/dist/jasmine-patch.js',
                'node_modules/zone.js/dist/async-test.js',
                'node_modules/zone.js/dist/fake-async-test.js',
               // RxJs.
               { pattern: 'node_modules/rxjs/**/*.js', included: false, watched: false },
               { pattern: 'node_modules/rxjs/**/*.js.map', included: false, watched: false },
               
               // paths loaded via module imports
               // Angular itself
               'karma-test-shim.js',
               { pattern: 'node_modules/@angular/**/*.js', included: false, watched: true },
               { pattern: 'node_modules/@angular/**/*.js.map', included: false, watched: false },
               
               { pattern: 'wwwroot/app/**/*.js', included: false, watched: true },
               { pattern: 'wwwroot/app/**/*.html', included: false, watched: true },
              

                //// paths to support debugging with source maps in dev tools
               { pattern: 'wwwroot/app/**/*.ts', included: false, watched: false },
               { pattern: 'wwwroot/app/**/*.js.map', included: false, watched: false },
        ],


        // list of files to exclude
        exclude: [
        ],


        //// preprocess matching files before serving them to the browser
        //// available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
        preprocessors: {
            'wwwroot/app/**/!(*spec).js': ['coverage']
        },


        // test results reporter to use
        // possible values: 'dots', 'progress'
        // available reporters: https://npmjs.org/browse/keyword/karma-reporter
        reporters: ['progress', 'dots','coverage'],

        coverageReporter: {
            reporters:[
                { type: 'lcov', subdir: '.', file: 'lcov.info' }]
        },

        // web server port
        port: 9876,


        // enable / disable colors in the output (reporters and logs)
        colors: true,


        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,


        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,


        // start these browsers
        // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
        browsers: ['Chrome'],

        // proxied base paths
        proxies: {
            // required for component assests fetched by Angular's compiler
            "/app/": "/base/wwwroot/app/"
        },
        // Continuous Integration mode
        // if true, Karma captures browsers, runs the tests and exits
        singleRun: true,

        // Concurrency level
        // how many browser should be started simultaneous
        concurrency: Infinity,

        customLaunchers: {
            Chrome_travis_ci: {
                base: 'Chrome',
                flags: ['--no-sandbox']
            }
        },

        // Karma plugins loaded
        plugins: [
            'karma-jasmine',
            'karma-chrome-launcher',
            'karma-coverage'
        ],
    })

    if (process.env.TRAVIS || process.env.CIRCLECI) {
        config.browsers = ['Chrome_travis_ci'];
        config.singleRun = true;
        config.browserNoActivityTimeout = 90000;
    }

}
