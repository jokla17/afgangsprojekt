import { Component, Inject, OnDestroy, OnInit, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Optional} from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router, RouterLinkActive, Routes } from '@angular/router';


export interface DialogData {
  label: string;
  content: string;
}

@Component({
  selector: 'app-map-dialog',
  templateUrl: './map-dialog.component.html',
  styleUrls: ['./map-dialog.component.scss']
})
export class MapDialogComponent implements OnInit, OnDestroy {

  selectedIndex = 0;

  isViewInitialized = false;

  isFormValid = false;

  navLinks = [
    {link: 'info', name: 'Info'},
    {link: 'commands', name: 'Commands'},
    {link: 'configuration', name: 'Configuration'},
  ];

  onNoClick(): void {
    //this.dialogRef.close();
  }

  selectedIndexChange(val: number) {
    this.selectedIndex = val;
    console.log('this is selected index: ', val);
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private changeDetector: ChangeDetectorRef,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any
  ) { }


  ngOnDestroy() {
    console.warn('---- Dialog was destroyed ----');
    this.router.navigate(['']);
  }

  ngOnInit() {
    console.log(this.navLinks);
    console.warn('----nav links founded: ', this.navLinks);
  }

  ngAfterViewInit() {
    this.isViewInitialized = true;
  }

  onFormChange(event: boolean) {
    console.log('this is event: ', event);
    this.isFormValid = event;
  }

  runCommand(){
    console.log(this.isFormValid);
  }


}
