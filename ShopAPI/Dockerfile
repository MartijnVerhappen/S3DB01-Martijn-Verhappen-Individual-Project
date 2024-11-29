FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore

WORKDIR /App/ShopAPI
RUN dotnet publish "ShopAPI.csproj" -c Release -o /App/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
EXPOSE 5000

COPY --from=build-env /App/out ./

ENV DB_SERVER=mysql-container
ENV DB_PORT=3306
ENV DB_NAME=webshop
ENV DB_USER=root
ENV DB_PASSWORD=Njitram0603

ENTRYPOINT ["dotnet", "ShopAPI.dll"]
