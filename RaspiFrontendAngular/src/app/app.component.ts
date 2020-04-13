import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  showDefault = true;
  title = 'RaspiFrontendAngular';

  constructor() {
  }

  div_clicked() {
    this.showDefault = !this.showDefault;
    console.log('div_clicked');
  }
}
