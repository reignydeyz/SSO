module.exports = {
  lintOnSave: false,
  configureWebpack: {
    devtool: 'source-map'
  }
}

process.env.VUE_APP_VERSION = require('./package.json').version