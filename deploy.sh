#!/bin/sh
set -e

$CIRCLE_BRANCH_U="${CIRCLE_BRANCH^^}"

cd ./Trappist/src/Promact.Trappist.Web
dotnet publish -c Release -o published
git log --format="%h" -n 1 > ./published/.revision

az login --service-principal -u $AZURE_USER -p $AZURE_PASSWORD --tenant $AZURE_TENANT

az storage blob upload-batch --destination $AZURE_CONTAINER_NAME$CIRCLE_BRANCH_U --source published/wwwroot

#Deploy to Azure

az webapp stop --name trappist-dev-win --resource-group trappist-dev-win

npm install ftp-deploy
node ftpDeploy.js

az webapp start --name trappist-dev-win --resource-group trappist-dev-win

#Deploy to Dockerhub
#cd ../../../
#docker build -t promact/trappist:$CIRCLE_BRANCH .
#docker login -u=$DOCKER_USERNAME -p=$DOCKER_PASSWORD
#docker push promact/trappist:$CIRCLE_BRANCH
