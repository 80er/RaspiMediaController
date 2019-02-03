import { Component, OnInit } from '@angular/core';
import { MessageQueueComponent } from 'src/app/message-queue/message-queue/message-queue.component';

@Component({
  selector: 'app-clock',
  templateUrl: './clock.component.html',
  styleUrls: ['./clock.component.css']
})
export class ClockComponent implements OnInit {

  public hour: string;
  public minute: string;
  public day: string;
  public dayname: string;
  public year: string;
  public month: string;
  constructor(private _messageQueue: MessageQueueComponent) { 
    this.hour = this.minute = this.day = this.year = this.month = "1";
    this.dayname = "Montag";
  }

  ngOnInit() {
    this._messageQueue.timeMessages.subscribe((data) => this.on_message(data));
  }

  private on_message = (...args: any[]) => {
    const message = args[0];
    console.info('in clock' + message);
    const data = JSON.parse(message);
    console.info('hour' + data.Hour);
    this.hour = data.Hour;
    this.minute = data.Minute;
    this.day = data.Day;
    this.dayname = data.Dayname;
    this.month = data.Month;
    this.year = data.Year;
  }
}
