/**
 * System configuration for Angular samples
 * Adjust as necessary for your application needs.
 */
(function (global) {
    System.config({
        baseURL: '/',
        defaultJSExtensions: true,
        paths: {
            // paths serve as alias
            'npm:': 'node_modules/'
        },
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


        // map tells the System loader where to look for things
        map: {
            // angular bundles
            '@angular/animations': 'npm:@angular/animations/bundles/animations.umd.js',
            '@angular/animations/browser': 'npm:@angular/animations/bundles/animations-browser.umd.js',
            '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
            '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
            '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
            '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
            '@angular/platform-browser/animations': 'npm:@angular/platform-browser/bundles/platform-browser-animations.umd.js',
            '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
            '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
            '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
            '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',
            '@angular/material': 'npm:@angular/material/bundles/material.umd.js',
            // other libraries
            'rxjs': 'npm:rxjs',
            'angular2-wizard-fix': 'npm:angular2-wizard-fix/dist/',
            'angular-in-memory-web-api': 'npm:angular-in-memory-web-api/bundles/in-memory-web-api.umd.js',
            'clipboard': 'npm:clipboard/dist/clipboard.js',
            'ngx-clipboard': 'npm:ngx-clipboard',
            'md2': 'npm:md2/bundles/md2.umd.js',
            'ng2-ckeditor': 'npm:ng2-ckeditor',
            'angular2-infinite-scroll': 'npm:angular2-infinite-scroll',
            'angular2-tinymce': 'npm:angular2-tinymce/dist',
            'tinymce': 'npm:tinymce',
            'ngx-popover': 'npm:ngx-popover',
            'chart.js': 'npm:chart.js',
            'ng2-charts': 'npm:ng2-charts',
            '@angular/cdk': 'npm:@angular/cdk/bundles/cdk.umd.js',
            'jspdf': 'npm:jspdf/dist/jspdf.debug.js',
            'ng2-ace-editor': 'npm:ng2-ace-editor',
            'brace': 'npm:brace',
            'w3c-blob': 'npm:w3c-blob',
            'buffer': 'npm:buffer',
            'ieee754': 'npm:ieee754',
            'base64-js': 'npm:base64-js'

            'jsPdfautoTable': 'npm:jspdf-autotable/dist/jspdf.plugin.autotable.js',
            'exceljs': 'npm:exceljs/dist/exceljs.js',
            'filesaver':'npm:file-saver'
        },
        // packages tells the System loader how to load when no filename and/or no extension
        packages: {
            rxjs: {
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
                'main': 'ng2-charts.js',
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
            brace: {
                main: 'index.js',
                defaultExtension: 'js'
            },
            'w3c-blob': {
                main: 'index.js',
                defaultExtension: 'js'
            },
            buffer: {
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
            'exceljs.js': {
                main: 'dist/exceljs.js',
                defaultExtension: 'js'
            },
            'filesaver': {
                main: 'FileSaver.js',
                defaultExtension: 'js'
            },
            '.': {
                defaultExtension: 'js'
            }
        }
    });
})(this);