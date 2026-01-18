# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers csproj et restaurer les dépendances
COPY ["src/GestMatch.Api/GestMatch.Api.csproj", "GestMatch.Api/"]
COPY ["src/GestMatch.Application/GestMatch.Application.csproj", "GestMatch.Application/"]
COPY ["src/GestMatch.Domain/GestMatch.Domain.csproj", "GestMatch.Domain/"]
COPY ["src/GestMatch.Infrastructure/GestMatch.Infrastructure.csproj", "GestMatch.Infrastructure/"]

RUN dotnet restore "GestMatch.Api/GestMatch.Api.csproj"

# Copier tout le code source et compiler
COPY src/ .
WORKDIR "/src/GestMatch.Api"
RUN dotnet build "GestMatch.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "GestMatch.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Installer les dépendances pour PostgreSQL
RUN apt-get update && apt-get install -y \
    libgdiplus \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestMatch.Api.dll"]
