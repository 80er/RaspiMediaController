import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageQueueComponent } from './message-queue/message-queue.component';

@NgModule({
  declarations: [MessageQueueComponent],
  imports: [
    CommonModule
  ],
  exports: [
    MessageQueueComponent
  ]
})
export class MessageQueueModule { }
