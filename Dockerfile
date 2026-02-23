# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Maraphon.sln", "./"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]

# Restore dependencies
RUN dotnet restore

# Copy all files and build
COPY . .
RUN dotnet build "WebApi/WebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Create folder for profiles and other uploads
RUN mkdir -p /app/wwwroot/profiles && chmod -R 777 /app/wwwroot

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]
