# build environment
FROM node:20-alpine3.17 as build

WORKDIR /app
COPY frontend/socialnetfrontend/package.json ./
COPY frontend/socialnetfrontend/package-lock.json ./
#RUN npm ci --silent
RUN npm install
RUN npm install react-scripts@3.4.1 -g --silent
COPY frontend/socialnetfrontend/. ./
RUN npm run build

# production environment
FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
# new
COPY frontend/socialnetfrontend/nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]