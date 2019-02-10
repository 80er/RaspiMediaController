FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
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
