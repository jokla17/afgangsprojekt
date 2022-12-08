import { Component, Input, OnInit } from '@angular/core';
import { Compumat } from './compumat';
import { CompumatService } from './compumat.service';

@Component({
  selector: 'app-compumat',
  templateUrl: './compumat.component.html',
  styleUrls: ['./compumat.component.scss']
})

//TODO: Probably remove this component?
export class CompumatComponent implements OnInit {

  @Input() compumat: Compumat;
  compumatList: Compumat[] = [];

  constructor(
    private compumatService: CompumatService,
  ) { }

  ngOnInit(): void {

  }

  // Get one specific compumat-device
  getCompumat(id: string): void {
    // Call getCompumat from service
    // Find index of compumat with given id in current list
    // Replace compumat and index with new compumat IF it exists
    // Else insert new compumat
    this.compumatService.getCompumat(id).subscribe(compuMat => {
      let currentCompumat = this.compumatList.find(compuMat => compuMat.id == id);
      if (currentCompumat !== undefined){
        let compumatIndex = this.compumatList.indexOf(currentCompumat);
        this.compumatList[compumatIndex] = compuMat;
      } else {
        this.compumatList.push(compuMat);
      }
    });
  }

  // Get list of all compumat devices
  getCompumats(): void {
    // Inserts all compumats into current list of compumats
    this.compumatService.getCompumats().subscribe(compumats => {
      this.compumatList = compumats;
    });
  }

}
