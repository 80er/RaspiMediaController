FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY MediaControllerBackendServices/MediaControllerBackendServices.csproj MediaControllerBackendServices/
RUN dotnet restore MediaControllerBackendServices/MediaControllerBackendServices.csproj
COPY . .
WORKDIR /src/MediaControllerBackendServices
RUN dotnet build MediaControllerBackendServices.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MediaControllerBackendServices.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MediaControllerBackendServices.dll"]
