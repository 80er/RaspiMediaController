import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RaumfeldComponent} from './raumfeld/raumfeld.component';


@NgModule({
  declarations: [RaumfeldComponent],
  imports: [
    CommonModule
  ],
  exports : [
    RaumfeldComponent
  ]
})
export class RaumfeldModule { }
