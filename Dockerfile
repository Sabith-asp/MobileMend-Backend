# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Common/Common.csproj Common/

RUN dotnet restore

COPY . .
WORKDIR /app/API
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Optional: Install DotNetEnv if you read from .env at runtime
COPY --from=build /out .

ENTRYPOINT ["dotnet", "API.dll"]
