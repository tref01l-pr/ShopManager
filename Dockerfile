FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShopManager.API/ShopManager.API.csproj", "ShopManager.API/"]
COPY ["ShopManager.Application/ShopManager.Application.csproj", "ShopManager.Application/"]
COPY ["ShopManager.DataAccess.SqlServer/ShopManager.DataAccess.SqlServer.csproj", "ShopManager.DataAccess.SqlServer/"]
COPY ["ShopManager.Domain/ShopManager.Domain.csproj", "ShopManager.Domain/"]
RUN dotnet restore "ShopManager.API/ShopManager.API.csproj"
COPY . .
WORKDIR "/src/ShopManager.API"
RUN dotnet build "ShopManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShopManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopManager.API.dll"]
