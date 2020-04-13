import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeatherComponent } from './weather/weather.component';

@NgModule({
  declarations: [WeatherComponent],
  imports: [
    CommonModule
  ],
  exports: [
    WeatherComponent
  ]
})
export class WeatherModule {
  constructor (@Optional() @SkipSelf() parentModule?: WeatherModule) {
    if (parentModule) {
      throw new Error(
        'WeatherModule is already loaded. Import it in the AppModule only');
    }
  }
}
