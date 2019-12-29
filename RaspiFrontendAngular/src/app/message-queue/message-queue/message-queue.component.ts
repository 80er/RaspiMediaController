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

  public timeMessages: Subject<string>;
  public weatherMessage: Subject<string>;

  status: Array<string> = [];
  private client: mqtt.Client;
  constructor() {
    this.timeMessages = new Subject<string>();
    this.weatherMessage = new Subject<string>();
    const options: mqtt.IClientOptions = {
      'keepalive': 5000,
      'reconnectPeriod': 10000,
      'clientId': 'RaspiWeatherStation',
      'host': process.env['MQTT_SERVER'],
      'port': 9001
    };

    // Create the client and listen for its connection
    this.client = mqtt.connect( options);
    this.client.subscribe('time_data');
    this.client.subscribe('weather_data');
    this.client.addListener('message', this.on_message);
    this.client.on('connect', this.on_connect);
   }

  ngOnInit() {
  }

  private send_weather_request = (...args: any[]) => {
    this.client.publish('weather_data', 'resend_all');
  }

  private send_time_request = (...args: any[]) => {
    this.client.publish('time_data', 'resend_all');
  }

  private on_connect = (...args: any[]) => {
    this.send_weather_request();
    this.send_time_request();
  }

  private on_message = (...args: any[]) => {
    const topic = args[0],
      message = args[1],
      packet: mqtt.Packet = args[2];
    if (topic === 'time_data') {
      this.timeMessages.next(message.toString());
    }
    else if(topic === 'weather_data') {
      this.weatherMessage.next(message.toString());
    } else {
      console.warn('Message from unknown topic received: ' + topic);
    }
  }
}
