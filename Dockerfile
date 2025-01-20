FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/EsriStatesApi.csproj", "Api/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["RestClients/RestClients.csproj", "RestClients/"]
RUN dotnet restore "./Api/EsriStatesApi.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "./EsriStatesApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EsriStatesApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV "ASPNETCORE_ENVIRONMENT"="Development"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EsriStatesApi.dll"]