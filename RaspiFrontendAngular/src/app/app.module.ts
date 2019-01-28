import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
//import { NgxMqttClientModule } from 'ngx-mqtt-client';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
    // NgxMqttClientModule.withOptions({
    //   clientId: "RaspiFrontend",
    //   clean: true,
    //   protocol: "tcp",
    //   host: 'localhost',
    //   port: 1883
    // })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
