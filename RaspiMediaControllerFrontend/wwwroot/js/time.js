const connection = new signalR.HubConnection(
    "http://localhost:5000/timehub", { logger: signalR.LogLevel.Information });

connection.on("UpdateTime", (hour, minute, second) => {
    document.getElementById("hour").innerHTML = hour;
    document.getElementById("minute").innerHTML = minute;
    document.getElementById("second").innerHTML = second;
});

connection.start().catch(err => console.error);