import { Component } from '@angular/core';
import * as mqtt from 'mqtt';
import { Subject } from 'rxjs/Rx';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'RaspiFrontendAngular';
  public messages: Subject<mqtt.Packet>;

  status: Array<string> = [];

  private client: mqtt.Client;

  constructor(/*private _mqttService: MqttService*/) {
    this.messages = new Subject<mqtt.Packet>();
    const options: mqtt.IClientOptions = {
      'keepalive': 5000,
      'reconnectPeriod': 10000,
      'clientId': 'RASPITV',
      'host': 'localhost',
      'port': 9001
    };

    // Create the client and listen for its connection
    this.client = mqtt.connect( options);
    this.client.subscribe('time_data');
    this.client.subscribe('weather_data');
    this.client.addListener('message', this.on_message);
}

public on_message = (...args: any[]) => {

  const topic = args[0],
    message = args[1],
    packet: mqtt.Packet = args[2];
  if (message.toString()) {
    console.warn(message.toString());
    status = message.toString();
    this.messages.next(message);
  } else {
    console.warn('Empty message received!');
  }
}
}
