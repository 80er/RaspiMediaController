# base image
FROM node:12.14.0 as build

# set working directory
RUN mkdir /usr/src/app
WORKDIR /usr/src/app

# add `/usr/src/app/node_modules/.bin` to $PATH
ENV PATH /usr/src/app/node_modules/.bin:$PATH

# install and cache app dependencies
COPY package.json /usr/src/app/package.json
RUN npm install
RUN npm install -g @angular/cli@8.3.21

# add app
COPY . /usr/src/app

# generate build
RUN npm run build --prod

##################
### production ###
##################

# base image
FROM tobi312/rpi-nginx

# copy artifact build from the 'build environment'
COPY --from=build /usr/src/app/dist /var/www/html

# expose port 80
EXPOSE 80

# run nginx
CMD ["nginx", "-g", "daemon off;"]
