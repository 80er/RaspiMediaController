FROM mcr.microsoft.com/dotnet/runtime:6.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY MediaControllerBackendServices.csproj MediaControllerBackendServices/
RUN dotnet restore MediaControllerBackendServices/MediaControllerBackendServices.csproj
COPY . MediaControllerBackendServices
WORKDIR /src/MediaControllerBackendServices
RUN dotnet build MediaControllerBackendServices.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MediaControllerBackendServices.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MediaControllerBackendServices.dll"]
