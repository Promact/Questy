/// <binding AfterBuild='sass' />

var gulp = require('gulp');
var concat = require("gulp-concat");
var uglify = require('gulp-uglify');
var sass = require('gulp-sass');
var Builder = require('systemjs-builder');
var inlineNg2Template = require("gulp-inline-ng2-template");
var ts = require("gulp-typescript");
var tsProject = ts.createProject("./wwwroot/tsconfig.json");
var runSequence = require('run-sequence');
var tslint = require('gulp-tslint');
var ngc = require('gulp-ngc');
var rollup = require('rollup-stream');
var source = require('vinyl-source-stream');


//production publish task
gulp.task('prod', function (done) {
    runSequence('ngc', 'bundle-shims', 'bundle-app', 'bundle-setup-app', 'bundle-conduct-app', 'sass', 'bundle-css', function () {
        done();
    });
});

gulp.task('lint', function (done) {
    gulp.src("./wwwroot/app/**/*.ts")
        .pipe(tslint({formatter:"stylish"}))
        .pipe(tslint.report());
});

//sass to css
gulp.task("sass", function () {
    return gulp.src('./wwwroot/css/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/css'));
});

//sass to css continuous watch
gulp.task('watch', ['sass'], function () {
    gulp.watch('./wwwroot/css/*.scss', ['sass']);
});

//Bundle all css into bundle.css
//TODO: also minify it, and maybe solve relative import errors if any
gulp.task("bundle-css", function () {
    return gulp.src([
        './node_modules/bootstrap/dist/css/bootstrap.css',
        './node_modules/@angular/material/prebuilt-themes/indigo-pink.css',
        './wwwroot/css/style.css'
    ])
    .pipe(concat('bundle.css'))
    .pipe(gulp.dest('./wwwroot/dist'));
});

//Converts ts files to js with html template inline
gulp.task("tstojs", function () {
    return tsProject.src()
        .pipe(inlineNg2Template({ base: './wwwroot/', useRelativePaths: true }))
        .pipe(tsProject())
        .js.pipe(gulp.dest("./wwwroot/app"));
});

//bundle shim files
gulp.task('bundle-shims', function () {
    return gulp.src(['./node_modules/core-js/client/shim.js',
        './node_modules/zone.js/dist/zone.js',
        './node_modules/clipboard/dist/clipboard.js',
        './node_modules/jspdf/dist/jspdf.debug.js',
        './node_modules/jspdf-autotable/dist/jspdf.plugin.autotable.js',
        './node_modules/filesaver/FileSaver.js'
    ])
    .pipe(concat('shims.js'))
    .pipe(uglify())
    .pipe(gulp.dest('./wwwroot/dist'));
});

//bundle main dashboard app
gulp.task('ngc', function (done) {
    return ngc('./wwwroot/tsconfig.aot.json');
});

gulp.task('bundle-app', () => {
    return rollup('wwwroot/rollup-config.js')
        .pipe(source('app.bundle.js'))
        .pipe(gulp.dest('./wwwroot/dist'));
});

//bundle setup app
gulp.task('bundle-setup-app', function (done) {
    return rollup('wwwroot/rollup-config.setup.js')
        .pipe(source('app.setup.bundle.js'))
        .pipe(gulp.dest('./wwwroot/dist'));
});

//bundle conduct app
gulp.task('bundle-conduct-app', function (done) {
    return rollup('wwwroot/rollup-config.conduct.js')
        .pipe(source('app.conduct.bundle.js'))
        .pipe(gulp.dest('./wwwroot/dist'));
});