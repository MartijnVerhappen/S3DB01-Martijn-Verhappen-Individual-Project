FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
EXPOSE 8080

COPY .. ./

RUN dotnet restore

WORKDIR /App/WebShopAPI
RUN dotnet publish "WebShopAPI.csproj" -c Release -o /App/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

ENTRYPOINT ["dotnet", "WebShopAPI.dll"]