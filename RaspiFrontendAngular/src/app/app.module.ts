import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MessageQueueModule } from './message-queue/message-queue.module';
import { ClockModule } from './clock/clock.module';
import { WeatherModule } from './weather/weather.module';
import { RaumfeldModule } from './raumfeld/raumfeld.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MessageQueueModule,
    ClockModule,
    WeatherModule,
    RaumfeldModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
