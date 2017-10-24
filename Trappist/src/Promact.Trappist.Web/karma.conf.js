module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine'],

        plugins: [
            require('karma-jasmine'),
            require('karma-chrome-launcher'),
            require('karma-coverage'),
            require('karma-jasmine-html-reporter')
        ],
        files: [

            'node_modules/systemjs/dist/system.src.js',
            //// Polyfills
            'node_modules/core-js/client/shim.js',
            'node_modules/reflect-metadata/Reflect.js',

            // zone.js
            'node_modules/zone.js/dist/zone.js',
            'node_modules/zone.js/dist/long-stack-trace-zone.js',
            'node_modules/zone.js/dist/proxy.js',
            'node_modules/zone.js/dist/sync-test.js',
            'node_modules/zone.js/dist/jasmine-patch.js',
            'node_modules/zone.js/dist/async-test.js',
            'node_modules/zone.js/dist/fake-async-test.js',


            { pattern: 'node_modules/rxjs/**/*.js', included: false, watched: false },

            'karma-test-shim.js',
            { pattern: 'node_modules/@angular/**/*.js', included: false, watched: false },
            { pattern: 'wwwroot/app/**/*.js', included: false, watched: true },
            { pattern: 'wwwroot/app/**/*.ts', included: false, watched: true },
            { pattern: 'wwwroot/app/**/*.js.map', included: false, watched: true },
            { pattern: 'wwwroot/app/**/*.html', included: false, watched: true },
            { pattern: 'wwwroot/**/*.css', included: false, watched: true },
            { pattern: 'node_modules/md2/bundles/md2.umd.js', included: false, watched: false },
            { pattern: 'node_modules/ngx-popover/**/*.js', included: false, watched: false },
            { pattern: 'node_modules/ngx-clipboard/**/*.js', included: false, watched: false },
            { pattern: 'node_modules/clipboard/dist/clipboard.js', included: false, watched: false },
            { pattern: 'node_modules/angular2-infinite-scroll/**/*.js', included: false, watched: false },
            { pattern: 'node_modules/tinymce/**/*.js', included: false, watched: false },
            { pattern: 'node_modules/angular2-tinymce/dist/**/*.js', included: false, watched: false }

            
        ],

        mime: {
            'text/x-typescript': ['ts', 'tsx']
        },

        
        proxies: {
            "/app/": "/base/wwwroot/app/"
        },

        //preprocessors: {
        //    // source files, that you wanna generate coverage for 
        //    // do not include tests or libraries 
        //    // (these files will be instrumented by Istanbul) 
        //    'wwwroot/app/**/!(*spec).js': ['coverage']
        //},

        client: {
            clearContext: false // leave Jasmine Spec Runner output visible in browser
        },
        reporters: ['progress', 'coverage','kjhtml'],

        coverageReporter: {
            dir: 'coverage/',
            reporters: [
                { type: 'html' },
                { type: 'lcov' }
            ]
        },

        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['Chrome'],
        singleRun: false
    });

};