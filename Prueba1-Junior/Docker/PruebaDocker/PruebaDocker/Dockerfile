# Imagen base solo con el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:7007
EXPOSE 7007

# Imagen con SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto y restaurar dependencias
COPY ["PruebaDocker.csproj", "./"]
RUN dotnet restore

# Copiar el resto del contenido
COPY . ./
RUN dotnet build -c Release -o /app/build

# Publicar la app
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PruebaDocker.dll"]
