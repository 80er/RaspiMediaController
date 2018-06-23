const connection = new signalR.HubConnection(
    "http://localhost:5000/timehub", { logger: signalR.LogLevel.Information });

connection.on("UpdateTime", (hour, minute) => {
    var newTime = String(hour).padStart(2, '0') + ":" + String(minute).padStart(2, '0');
    document.getElementById("timeString").innerHTML = newTime;
});

connection.start().catch(err => console.error);