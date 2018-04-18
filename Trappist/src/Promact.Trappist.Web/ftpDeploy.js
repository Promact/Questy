var FtpDeploy = require('ftp-deploy');
var ftpDeploy = new FtpDeploy();

var config = {
    username: process.env.FTP_USERNAME,
    password: process.env.FTP_PASSWORD,
    host: process.env.FTP_HOST,
    port: 21,
    localRoot: __dirname + "/published",
    remoteRoot: "/site/wwwroot",
    include: ['*']
}

ftpDeploy.deploy(config, function (err) {
    if (err) console.log(err)
    else console.log('Finished FTP Deployment');
});