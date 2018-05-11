var FtpDeploy = require('ftp-deploy');
var ftpDeploy = new FtpDeploy();

var config = {
    username: process.env["FTP_USERNAME_" + process.env.CIRCLE_BRANCH.toUpperCase()],
    password: process.env["FTP_PASSWORD_" + process.env.CIRCLE_BRANCH.toUpperCase()],
    host: process.env["FTP_HOST_" + process.env.CIRCLE_BRANCH.toUpperCase()],
    port: 21,
    localRoot: __dirname + "/published",
    remoteRoot: "/site/wwwroot",
    include: ['*']
}

ftpDeploy.deploy(config, function (err) {
    if (err) console.log(err)
    else console.log('Finished FTP Deployment');
});