/// <binding AfterBuild='sass' />

var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task("sass", function () {
    return gulp.src('./wwwroot/css/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/css'));
});