cd C:\Users\Chef\source\repos\80er\RaspiMediaController\RaspiFrontendAngular
docker buildx create --name armv7builder --use
docker buildx build --platform linux/arm/v7 -t 80er/netatmoweatherstationdisplay:LATEST --push .
cd C:\Users\Chef\source\repos\80er\RaspiMediaController\MediaControllerBackendServices
docker buildx build --platform linux/arm/v7 -t 80er/mediacontrollerbackendservice:LATEST --push .