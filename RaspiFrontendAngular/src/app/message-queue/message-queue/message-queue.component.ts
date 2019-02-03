import { Component, OnInit } from '@angular/core';
import {Injectable} from '@angular/core';
import * as mqtt from 'mqtt';
import { Subject } from 'rxjs/Rx';

@Injectable({
  providedIn:'root'
})

@Component({
  selector: 'app-message-queue',
  templateUrl: './message-queue.component.html',
  styleUrls: ['./message-queue.component.css']
})
export class MessageQueueComponent implements OnInit {
  
  public messages: Subject<mqtt.Packet>;
  status: Array<string> = [];
  private client: mqtt.Client;
  constructor() {
    this.messages = new Subject<mqtt.Packet>();
    const options: mqtt.IClientOptions = {
      'keepalive': 5000,
      'reconnectPeriod': 10000,
      'clientId': 'RaspiWeatherStation',
      'host': 'localhost',
      'port': 9001
    };

    // Create the client and listen for its connection
    this.client = mqtt.connect( options);
    this.client.subscribe('time_data');
    this.client.subscribe('weather_data');
    this.client.addListener('message', this.on_message);
   }

  ngOnInit() {
  }

  private on_message = (...args: any[]) => {

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
