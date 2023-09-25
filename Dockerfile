FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env

WORKDIR /app
COPY . .
RUN dotnet restore ./OpenTelemetrySpike1.sln --source https://api.nuget.org/v3/index.json
RUN dotnet publish ./OpenTelemetrySpike1.WebApplication1/OpenTelemetrySpike1.WebApplication1.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
ENV DOTNET_ENVIRONMENT=Production
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "OpenTelemetrySpike1.WebApplication1.dll"]
