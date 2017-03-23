FROM microsoft/aspnetcore:1.1
WORKDIR /app
COPY Trappist/src/Promact.Trappist.Web/published  ./
ENTRYPOINT ["dotnet","Promact.Trappist.Web.dll"]
