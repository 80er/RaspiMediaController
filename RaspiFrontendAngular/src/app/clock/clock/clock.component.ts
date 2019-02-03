import { Component, OnInit } from '@angular/core';
import { MessageQueueComponent } from 'src/app/message-queue/message-queue/message-queue.component';

@Component({
  selector: 'app-clock',
  templateUrl: './clock.component.html',
  styleUrls: ['./clock.component.css']
})
export class ClockComponent implements OnInit {

  constructor(private _messageQueue: MessageQueueComponent) { 
    
  }

  ngOnInit() {
    this._messageQueue.messages.subscribe((data) => this.on_message(data));
  }

  private on_message = (...args: any[]) => {
    const message = args[0];
    console.info('in clock' + message.toString());
  }
}
