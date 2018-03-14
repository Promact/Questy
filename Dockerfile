FROM microsoft/aspnetcore:2.0.0
WORKDIR /app
COPY Trappist/src/Promact.Trappist.Web/published  ./
ENTRYPOINT ["dotnet","Promact.Trappist.Web.dll"]
