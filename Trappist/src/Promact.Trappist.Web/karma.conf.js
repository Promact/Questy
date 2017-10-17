module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine'],

        plugins: [
            require('karma-jasmine'),
            require('karma-chrome-launcher'),
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
           
            { pattern: 'node_modules/angular2-wizard-fix/dist/index.js', included: false, watched: false },
            { pattern: 'node_modules/md2/bundles/md2.umd.js', included: false, watched: false },
            { pattern: 'node_modules/angular2-wizard-fix/dist/src/wizard.component.js', included: false, watched: false },
            { pattern: 'node_modules/angular2-wizard-fix/dist/src/wizard-step.component.js', included: false, watched: false }
        ],

        mime: {
            'text/x-typescript': ['ts', 'tsx']
        },


        proxies: {
            "/app/": "/base/wwwroot/app/"
        },


        client: {
            clearContext: false // leave Jasmine Spec Runner output visible in browser
        },
        reporters: ['progress', 'kjhtml'],

        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['Chrome'],
        singleRun: false
    });
};