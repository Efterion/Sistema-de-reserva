# Construccion usando Node
FROM node:20 AS build
WORKDIR /usr/src/app

# Copiamos el package.json e instalamos dependencias
COPY package*.json ./
RUN npm install

# Copiar el resto del codigo y construir la app

COPY . .
RUN npm run build --prod

#Servidor
FROM nginx:1.23-alpine
COPY --from=build /usr/src/app/dist/reservas-app/browser /usr/share/nginx/html

EXPOSE 80
