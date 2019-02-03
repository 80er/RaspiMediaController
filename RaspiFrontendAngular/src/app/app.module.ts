import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MessageQueueModule } from './message-queue/message-queue.module';
import { MessageQueueComponent } from './message-queue/message-queue/message-queue.component';
import { ClockModule } from './clock/clock.module';
import {ClockComponent} from './clock/clock/clock.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MessageQueueModule,
    ClockModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
