FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WeatherClient/WeatherClient.csproj", "WeatherClient/"]
RUN dotnet restore "WeatherClient/WeatherClient.csproj"
COPY . .
WORKDIR "/src/WeatherClient"
RUN dotnet build "WeatherClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherClient.dll"]
