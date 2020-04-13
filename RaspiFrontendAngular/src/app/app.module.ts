import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MessageQueueModule } from './message-queue/message-queue.module';
import { ClockModule } from './clock/clock.module';
import { WeatherModule } from './weather/weather.module';
import { RaumfeldComponent } from './raumfeld/raumfeld.component';

@NgModule({
  declarations: [
    AppComponent,
    RaumfeldComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MessageQueueModule,
    ClockModule,
    WeatherModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
