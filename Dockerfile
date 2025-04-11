FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/QuotBot.Api/QuotBot.Api.csproj", "QuotBot.Api/"]
COPY ["./src/QuotBot.Api", "./QuotBot.Api"]
COPY ["./src/QuotBot.Core", "./QuotBot.Core"]
RUN dotnet restore "./QuotBot.Api/QuotBot.Api.csproj"
WORKDIR "/src"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "QuotBot.Api/QuotBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM node:23-slim AS node
WORKDIR /src
COPY ["./src/QuotBot.Ui", "."]
RUN npm install
RUN yarn build

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS publish
RUN mkdir -p /data
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=node /src/dist ./wwwroot
ENTRYPOINT ["dotnet", "QuotBot.Api.dll"]
