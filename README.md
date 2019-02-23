# Netatmo Weatherstation Display
This project is interesting for people having a netatmo weatherstation. If you want to see your data permanently on a screen and do not want to use the App all the time this is for you.
As i also want to learn some things of course this is maybe a little over engineered. However. It works ;)

Architecture is in principle a .NET Core backend which connects the Netatmo API and distributes the information using a MQTT broker like Mosquitto.
the frontend is written in angular and is shown in an electron application which starts in kiosk mode. To make it very easy to start the stuff on the raspi both things are hosted in docker containers.


# Docker build commands in the corresponding directories
docker build -t weatherstation_frontend .
docker create --name weatherstation_frontend -p 4200:80 --restart=always weatherstation_frontend
docker build -t weatherstationbackend .
docker container create -e NETATMO_CLIENT_SECRET=YOUR_API_KEY -e NETATMO_CLIENT=YOUR_CLIENT_ID -e NETATMO_USER=NETATMO_USER -e NETATMO_PASSWORD=YOUR_PASSWORD -e NETATMO_DEVICE=MAC_OF_DEVICE --name=weatherstation_backend
