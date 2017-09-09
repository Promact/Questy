const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const AotPlugin = require('@ngtools/webpack').AotPlugin;
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;

module.exports = (env) => {

    const isDevBuild = !(env && env.prod);

    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleOutputDir = './wwwroot/dist';

    const clientBundleConfig = {
        stats: { modules: false },
        node: {
            fs: 'empty'
        },
        context: __dirname,
        resolve: { extensions: ['.js', '.ts'] },
        entry: {
            'main': './wwwroot/app/main.ts',
            'main.setup': './wwwroot/app/main.setup.ts',
            'main.conduct': './wwwroot/app/main.conduct.ts'
        },
        output: {
            filename: '[name].js',
            publicPath: '/dist/', // Webpack dev middleware, if enabled, handles requests for this URL prefix
            path: path.join(__dirname, clientBundleOutputDir)
        },
        module: {
            rules: [
                { test: /\.ts$/, include: [/wwwroot/, /node_modules\/ngx-clipboard/], use: isDevBuild ? ['awesome-typescript-loader?silent=true&configFileName=wwwroot/tsconfig.json', 'angular2-template-loader'] : '@ngtools/webpack' },
                { test: /\.html$/, use: 'html-loader?minimize=false' },
                { test: /\.scss$/, loaders: ['raw-loader', 'sass-loader']},
                { test: /\.css$/, use: ['to-string-loader', isDevBuild ? 'css-loader' : 'css-loader?minimize'] },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
            ]
        },
        plugins: [
            new CheckerPlugin(),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/dist/vendor-manifest.json')
            })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [
                // Plugins that apply in production builds only
                new webpack.optimize.UglifyJsPlugin(),
                new AotPlugin({
                    tsConfigPath: './wwwroot/tsconfig.json',
                    entryModule: path.join(__dirname, 'wwwroot/app/app.module#AppModule')
                })
            ])
    };
   
    return [clientBundleConfig];
};
