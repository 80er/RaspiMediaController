import { Component } from '@angular/core';
import * as mqtt from 'mqtt';
import { Subject } from 'rxjs/Rx';
//import { MessageQueueComponent } from './message-queue/message-queue/message-queue.component';
import { ClockComponent } from './clock/clock/clock.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'RaspiFrontendAngular';

  constructor() {
  }
}
