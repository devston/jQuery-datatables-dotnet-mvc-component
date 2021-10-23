﻿const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

const bundleFileName = "bundle";
const dirName = "dist";

module.exports = (env, argv) => {
    return {
        mode: argv.mode === "production" ? "production" : "development",
        entry: ["./node_modules/bootstrap/dist/js/bootstrap.bundle.js", "./Content/app.scss"],
        devtool: false,
        output: {
            filename: bundleFileName + ".js",
            path: path.resolve(__dirname, dirName)
        },
        module: {
            rules: [
                {
                    test: /\.s[c|a]ss$/,
                    use:
                        [
                            "style-loader",
                            MiniCssExtractPlugin.loader,
                            "css-loader",
                            {
                                loader: "postcss-loader", // Run postcss actions.
                                options: {
                                    postcssOptions: {
                                        plugins: function () {
                                            // postcss plugins, can be exported to postcss.config.js
                                            let plugins = [require("autoprefixer")];

                                            if (argv.mode === "production") {

                                                plugins.push(require("cssnano"));
                                            }

                                            return plugins;
                                        }
                                    }
                                }
                            },
                            "sass-loader"
                        ]
                }
            ]
        },
        plugins: [
            new CleanWebpackPlugin(),
            new MiniCssExtractPlugin({
                filename: bundleFileName + ".css"
            })
        ]
    };
};