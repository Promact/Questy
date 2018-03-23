FROM microsoft/aspnetcore:2
WORKDIR /app
COPY Trappist/src/Promact.Trappist.Web/published  ./
ENTRYPOINT ["dotnet","Promact.Trappist.Web.dll"]
