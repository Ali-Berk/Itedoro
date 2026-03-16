# Temel katman (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# İnşa katmanı (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0.103 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Proje dosyalarını kendi katman isimlendirmene (API, Business, Data) göre kopyala
COPY ["Itedoro.Api/Itedoro.Api.csproj", "src/Itedoro.Api/"]
COPY ["Itedoro.Business/Itedoro.Business.csproj", "src/Itedoro.Business/"]
COPY ["Itedoro.Data/Itedoro.Data.csproj", "src/Itedoro.Data/"]

# Restore işlemi (API katmanı üzerinden)
RUN dotnet restore "./src/Itedoro.Api/Itedoro.Api.csproj"

# Kalan tüm dosyaları kopyala
COPY . .
WORKDIR "/src/Itedoro.Api"

# Projeyi derle
RUN dotnet build "./Itedoro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish işlemi
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Itedoro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final katmanı (Çalıştırma)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Itedoro.Api.dll"]