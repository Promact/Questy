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

//production publish task
gulp.task('prod', function (done) {
    runSequence('tstojs', 'bundle-shims', 'bundle-app', 'bundle-setup-app', 'sass', function () {
        done();
    });
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
        './node_modules/zone.js/dist/zone.js'])
    .pipe(concat('shims.js'))
    .pipe(uglify())
    .pipe(gulp.dest('./wwwroot/'));
});

//bundle main dashboard app
gulp.task('bundle-app', function (done) {

    var builder = new Builder('./', './wwwroot/systemjs.config.js');

    builder
      .buildStatic('./wwwroot/app/main.js', './wwwroot/app-bundle.js', {
          runtime: false
          /*minify: true,
          mangle: false*/
      }).then(function () {
          done();
      });
});

//bundle setup app
gulp.task('bundle-setup-app', function (done) {

    var builder = new Builder('./', './wwwroot/systemjs.config.js');

    builder
      .buildStatic('./wwwroot/app/main-setup.js', './wwwroot/setup-app-bundle.js', {
          runtime: false
          /*minify: true,
          mangle: false*/
      }).then(function () {
          done();
      });
});