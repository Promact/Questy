#!/bin/sh
set -e

CIRCLE_BRANCH_U="${CIRCLE_BRANCH^^}"

cd ./Trappist/src/Promact.Trappist.Web
dotnet publish -c Release -o published
git log --format="%h" -n 1 > ./published/.revision

az login --service-principal -u $AZURE_USER -p $AZURE_PASSWORD --tenant $AZURE_TENANT

#Deploy to Azure

az webapp stop --name trappist-dev-win --resource-group trappist-dev

npm install ftp-deploy@1.2.2
node ftpDeploy.js

az webapp start --name trappist-dev-win --resource-group trappist-dev

#Deploy to Dockerhub
#cd ../../../
#docker build -t promact/trappist:$CIRCLE_BRANCH .
#docker login -u=$DOCKER_USERNAME -p=$DOCKER_PASSWORD
#docker push promact/trappist:$CIRCLE_BRANCH
