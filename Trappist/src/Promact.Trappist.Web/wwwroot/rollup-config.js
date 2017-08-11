import nodeResolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import uglify from 'rollup-plugin-uglify';

export default {
    entry: 'wwwroot/app/main.aot.js',
    sourceMap: false,
    format: 'iife',
    onwarn: function (warning) {
        // Skip certain warnings

        // should intercept ... but doesn't in some rollup versions
        if (warning.code === 'THIS_IS_UNDEFINED') { return; }

        // console.warn everything else
        console.warn(warning.message);
    },
    external: [
        'clipboard'
    ],
    globals: {
        'clipboard': 'Clipboard'
    },
    plugins: [
        nodeResolve({ jsnext: true, module: true }),
        commonjs({
            include: ['node_modules/**'],

			namedExports: {
                'node_modules/chart.js/dist/Chart.js': ['chart.js'],
                'node_modules/ng2-charts/index.js': ['ChartsModule']
			}
        }),
		uglify()
    ]
}