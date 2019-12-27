import { Component, OnInit } from '@angular/core';
import { MessageQueueComponent } from 'src/app/message-queue/message-queue/message-queue.component';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})

export class WeatherComponent implements OnInit {

  public humidity: string;
  public temperature: string;
  public pressure: string;

  public modules: Array<WeatherModule>;

  constructor(private _messageQueue: MessageQueueComponent) {
    this.modules = new Array<WeatherModule>();
    this.humidity = '52';
    this.temperature = '21.5';
    this.pressure = '1042.3';
   }

  ngOnInit() {
    this._messageQueue.weatherMessage.subscribe((data) => this.on_message(data));
  }

  private on_message = (...args: any[]) => {
    const message = args[0];
    if(message === 'resend_all') {
       return;
    }
    const data = JSON.parse(message);
    if (data.Type === 1) {
      this.humidity = data.Humidity;
      this.temperature = data.Temperature;
    } else if (data.Type === 2 || data.Type === 0) {
      let found = false;
      this.modules.forEach(element => {
        if (element.name === data.Name) {
          element.humidity = data.Humidity;
          element.temperature = data.Temperature;
          element.co2 = data.CO2;
          if (data.Type === 0) {
            element.pressure = data.Pressure;
            this.pressure = element.pressure;
          }
          found = true;
        }
      });
      if(!found) {
        this.modules.push(new WeatherModule(data));
      }
    } else {
      console.info('not supported module type: ' + data.Type);
    }
  }
}

class WeatherModule {
  public name: string;
  public humidity: string;
  public temperature: string;
  public co2: string;
  public type: string;
  public pressure: string;

  constructor(data) {
    this.name = data.Name;
    this.humidity = data.Humidity;
    this.temperature = data.Temperature;
    this.co2 = data.CO2;
    this.type = data.Type;
    if (data.Type === 0) {
      this.pressure = data.Pressure;
    }
  }
}
