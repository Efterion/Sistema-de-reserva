# Etapa 1: Construcción (CON NODE.JS AÑADIDO)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
 
# --- ¡LA SOLUCIÓN ESTÁ AQUÍ! ---
# 1. Actualizar repositorios e instalar curl
RUN apt-get update && apt-get install -y curl
# 2. Añadir el repositorio oficial de Node.js (v20 LTS, el que usa tu frontend)
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
# 3. Instalar Node.js
RUN apt-get install -y nodejs
# --- FIN DE LA INSTALACIÓN DE NODE.JS ---
 
# --- (Ahora sigue la estrategia de "restaurar por proyecto") ---
# 4. Copiamos los .csproj PRIMERO, uno por uno
COPY BackEnd/Reservas.domain/Reservas.domain.csproj BackEnd/Reservas.domain/
COPY BackEnd/Reservas.Application/Reservas.Application.csproj BackEnd/Reservas.Application/
COPY BackEnd/Reservas.Infrastructure/Reservas.Infrastructure.csproj BackEnd/Reservas.Infrastructure/
COPY BackEnd/Reservas.API/Reservas.API.csproj BackEnd/Reservas.API/
COPY BackEnd/Reservas.sln BackEnd/
COPY FrontEnd/reservas-app/reservas-app.esproj FrontEnd/reservas-app/
 
# 5. Restauramos CADA proyecto individualmente
WORKDIR /src/BackEnd/Reservas.domain
RUN dotnet restore
WORKDIR /src/BackEnd/Reservas.Application
RUN dotnet restore
WORKDIR /src/BackEnd/Reservas.Infrastructure
RUN dotnet restore
WORKDIR /src/BackEnd/Reservas.API
RUN dotnet restore
# --- FIN DE LA ESTRATEGIA DE RESTORE ---
 
# 6. Copiamos TODO el código fuente (los .cs, etc.)
WORKDIR /src
COPY . .
 
# 7. Nos movemos a la carpeta de la API para publicar
WORKDIR /src/BackEnd/Reservas.API
# (Este comando ahora SÍ funcionará porque Node.js está instalado)
RUN dotnet publish -c Release -o /app/publish 
 
# Etapa 2: Ejecución (usando el runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Reservas.API.dll"]