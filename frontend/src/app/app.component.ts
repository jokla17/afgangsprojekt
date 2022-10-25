import { Component } from '@angular/core';
import { CompumatType } from './components/compumat/compumatType.enum';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'frontend';

  getType(): CompumatType{
    return CompumatType.GATE;
  }
}
