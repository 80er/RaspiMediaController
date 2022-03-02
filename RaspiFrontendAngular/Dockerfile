# base image
FROM node:17.6.0 as build

# set working directory
RUN mkdir /usr/src/app
WORKDIR /usr/src/app

ENV PATH /usr/src/app/node_modules/.bin:$PATH

# install and cache app dependencies
COPY package.json /usr/src/app/package.json
RUN npm install
RUN npm install -g @angular/cli@13.2.5

# add app
COPY . /usr/src/app

# generate build
RUN npm run build --prod

# base image
FROM nginx:1.21.6

# copy artifact build from the 'build environment'
COPY --from=build /usr/src/app/dist /usr/share/nginx/html

# expose port 80
EXPOSE 80
