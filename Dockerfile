# syntax=docker/dockerfile:1

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AD COURSEWORK 2.csproj", "./"]
RUN dotnet restore "AD COURSEWORK 2.csproj"

COPY . .
RUN dotnet publish "AD COURSEWORK 2.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 80

# Pass MySQL at runtime with:
#   -e ConnectionStrings__DefaultConnection="Server=...;Port=3306;Database=...;User=...;Password=...;TreatTinyAsBoolean=true;CharSet=utf8mb4;"
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AD COURSEWORK 2.dll"]
