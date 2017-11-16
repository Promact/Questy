Error.stackTraceLimit = 0;
jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000;

// (1)
var builtPaths = (__karma__.config.builtPaths || ['wwwroot/app/'])
    .map(function (p) { return '/base/' + p; });

// (2)
__karma__.loaded = function () { };



// (3)
function isJsFile(path) {
    return path.slice(-3) == '.js';
}

// (4)
function isSpecFile(path) {
    return /\.spec\.(.*\.)?js$/.test(path);
}

// (5)
function isBuiltFile(path) {
    return isJsFile(path) &&
        builtPaths.reduce(function (keep, bp) {
            return keep || (path.substr(0, bp.length) === bp);
        }, false);
}

// (6)
var allSpecFiles = Object.keys(window.__karma__.files)
    .filter(isSpecFile)
    .filter(isBuiltFile);



// (7)

SystemJS.config({
    baseURL: '/base',

    meta: {

        'node_modules/tinymce/plugins/advlist/plugin.js': { format: 'global' },
        'node_modules/tinymce/plugins/autoresize/plugin.js': { format: 'global' },
        'node_modules/tinymce/plugins/code/plugin.js': { format: 'global' },
        'node_modules/tinymce/plugins/link/plugin.js': { format: 'global' },
        'node_modules/tinymce/plugins/lists/plugin.js': { format: 'global' },
        'node_modules/tinymce/plugins/paste/plugin.js': { format: 'global' },
        'node_modules/tinymce/plugins/table/plugin.js': { format: 'global' },
        'node_modules/tinymce/themes/modern/theme.js': { format: 'global' }
    },

    map: {

        '@angular/core/testing': 'node_modules/@angular/core/bundles/core-testing.umd.js',
        '@angular/common/testing': 'node_modules/@angular/common/bundles/common-testing.umd.js',
        '@angular/compiler/testing': 'node_modules/@angular/compiler/bundles/compiler-testing.umd.js',
        '@angular/platform-browser/testing': 'node_modules/@angular/platform-browser/bundles/platform-browser-testing.umd.js',
        '@angular/platform-browser-dynamic/testing': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic-testing.umd.js',
        '@angular/animations': 'node_modules/@angular/animations/bundles/animations.umd.js',
        '@angular/animations/browser': 'node_modules/@angular/animations/bundles/animations-browser.umd.js',
        '@angular/core': 'node_modules/@angular/core/bundles/core.umd.js',
        '@angular/common': 'node_modules/@angular/common/bundles/common.umd.js',
        '@angular/compiler': 'node_modules/@angular/compiler/bundles/compiler.umd.js',
        '@angular/platform-browser': 'node_modules/@angular/platform-browser/bundles/platform-browser.umd.js',
        '@angular/platform-browser/animations': 'node_modules/@angular/platform-browser/bundles/platform-browser-animations.umd.js',
        '@angular/platform-browser-dynamic': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
        '@angular/http': 'node_modules/@angular/http/bundles/http.umd.js',
        '@angular/router': 'node_modules/@angular/router/bundles/router.umd.js',
        '@angular/forms': 'node_modules/@angular/forms/bundles/forms.umd.js',
        '@angular/material': 'node_modules/@angular/material/bundles/material.umd.js',
        // other libraries
        'rxjs': 'node_modules/rxjs',
        'angular2-wizard-fix': 'node_modules/angular2-wizard-fix/dist/',
        'angular-in-memory-web-api': 'node_modules/angular-in-memory-web-api/bundles/in-memory-web-api.umd.js',
        'clipboard': 'node_modules/clipboard/dist/clipboard.js',
        'ngx-clipboard': 'node_modules/ngx-clipboard',
        'md2': 'node_modules/md2/bundles/md2.umd.js',
        'ng2-ckeditor': 'node_modules/ng2-ckeditor',
        'angular2-infinite-scroll': 'node_modules/angular2-infinite-scroll',
        'angular2-tinymce': 'node_modules/angular2-tinymce/dist',
        'tinymce': 'node_modules/tinymce',
        'ngx-popover': 'node_modules/ngx-popover',
        'chart.js': 'node_modules/chart.js',
        'ng2-charts': 'node_modules/ng2-charts',
        '@angular/cdk': 'node_modules/@angular/cdk/bundles/cdk.umd.js',
        'jspdf': 'node_modules/jspdf/dist/jspdf.debug.js',
        'ng2-ace-editor': 'node_modules/ng2-ace-editor',
        'brace': 'node_modules/brace',
        'w3c-blob': 'node_modules/w3c-blob',
        'buffer': 'node_modules/buffer',
        'ieee754': 'node_modules/ieee754',
        'base64-js': 'node_modules/base64-js',
        'ace-builds': 'node_modules/ace-builds/src-min'

    },

    packages: {
        rxjs: {
            defaultExtension: 'js'
        },
        'rxjs/operators': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'angular2-wizard-fix': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'ngx-clipboard': {
            main: 'dist/bundles/ngxClipboard.umd.js'
        },
        'clipboard': {
            defaultExtension: 'js'
        },
        'ng2-ckeditor': {
            'main': 'lib/index.js',
            'defaultExtension': 'js'
        },
        'angular2-infinite-scroll': {
            main: 'src/index.js',
            defaultExtension: 'js'
        },
        'ng2-charts': {
            'main': 'index.js',
            'defaultExtension': 'js'
        },
        'angular2-tinymce': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'tinymce': { defaultExtension: 'js' },
        'chart.js': {
            main: 'dist/Chart.bundle.min.js',
            defaultExtension: 'js'
        },
        'ngx-popover': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'ng2-ace-editor': {
            main: 'ng2-ace-editor.js',
            defaultExtension: 'js'
        },
        'brace': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'w3c-blob': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'buffer': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'ieee754': {
            main: 'index.js',
            deafultExtension: 'js'
        },
        'base64-js': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        'ace-builds': {
            main: 'ace.js',
            defaultExtension: 'js'
        },
        '.': {
            defaultExtension: 'js'
        }
    },
});



// (9)


Promise.all([
    System.import('@angular/core/testing'),
    System.import('@angular/platform-browser-dynamic/testing')
])

    .then(function (providers) {
        var coreTesting = providers[0];
        var browserTesting = providers[1];

        coreTesting.TestBed.initTestEnvironment(
            browserTesting.BrowserDynamicTestingModule,
            browserTesting.platformBrowserDynamicTesting());
    }).then(


    function initTesting() {
        return Promise.all(
            allSpecFiles.map(function (moduleName) {
                return System.import(moduleName);
            }));

    }).then(__karma__.start, __karma__.error);

