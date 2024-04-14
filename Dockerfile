FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/API/App.API.csproj", "API/"]
COPY ["src/Application/App.Application.csproj", "Application/"]
COPY ["src/Domain/App.Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/App.Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "API/App.API.csproj"

COPY . .

WORKDIR /src/src/API

RUN dotnet build "App.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "App.API.dll"]
