import { Component, Input, OnInit } from '@angular/core';
import { Compumat } from '../../compumat/compumat';
import { CompumatType } from '../../compumat/compumatType.enum';

@Component({
  selector: 'app-boom-icon',
  templateUrl: './boom-icon.component.html',
  styleUrls: ['./boom-icon.component.scss']
})
export class BoomIconComponent implements OnInit {

  @Input() compumat: Compumat;
  fillColor = 'gray';
  constructor() { }

  ngOnInit(): void {
    // FOR TESTING:
    this.compumat = {
      icon: null,
      id: "01",
      latitude: null,
      longitude: null,
      name: "test",
      status: "ok",
      type: CompumatType.GATE,
    }
  }

  changeState(status: string): void {
    this.compumat.status = status;
  }
}
