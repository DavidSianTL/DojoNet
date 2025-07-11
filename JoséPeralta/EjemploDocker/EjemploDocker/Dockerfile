#Etapa 1: Construcción de la imagen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar los archivos del proyecto
COPY *.csproj ./

# Restaurar las dependencias del proyecto
RUN dotnet restore

# Copiar el resto de los archivos del proyecto
COPY . ./

# Construir la aplicación
RUN dotnet publish -c Release -o out


# Etapa 2: Ejecución de la imagen
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Copiar los archivos publicados desde la etapa de construcción
WORKDIR /app

# Copiar los archivos publicados desde la etapa de construcción
COPY --from=build /app/out .

# Exponer el puerto 80
EXPOSE 80

# Establecer el comando de inicio
ENTRYPOINT ["dotnet", "EjemploDocker.dll"]