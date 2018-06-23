const temperatureconnection = new signalR.HubConnection(
    "http://localhost:5000/temperaturehub", { logger: signalR.LogLevel.Information });

connection.on("UpdateTemperature", (temperature) => {
    var temperatureString = temperature.temperature.toFixed(2) + "° " + temperature.unit;
    document.getElementById("temperature").innerHTML = temperatureString;
});

temperatureconnection.start().catch(function () {
    document.getElementById("temperature").innerHTML = "Reconnect in 20s";
    setTimeout(function () {
        temperatureconnection.start().catch(err => console.error);
    }, 20000);
});

temperatureconnection.disconnected(function () {
        document.getElementById("temperature").innerHTML = "Reconnect in 10s";
        setTimeout(function () {
            temperatureconnection.start().catch(err => console.error);
        }, 10000);
    }
);