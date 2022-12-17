import { Component, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Compumat } from 'src/app/components/compumat/compumat';



@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})

export class InfoComponent implements OnInit {

  columnsToDisplay = ['Id', 'StationNo', 'Name', 'Latitude', 'Longitude', 'Type', 'Status'];
  @Input() compumat: any;
  compumatArray: any[] = [];

  constructor() {

  }

  ngOnInit(): void {
    this.compumatArray.push(this.compumat);
    console.log(this.compumat);
  }

}
