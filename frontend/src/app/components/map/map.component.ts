import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MapData } from './MapData';
import { MapService } from './map.service';
import { Compumat } from '../compumat/compumat';
import { CompumatType } from '../compumat/compumatType.enum';
import * as L from 'leaflet';
import { MarkerService } from './marker/marker.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CompumatService } from '../compumat/compumat.service';
import { MapDialogComponent } from './map-dialog/map-dialog.component';
import { Router } from '@angular/router';

let iconRetinaUrl = 'assets/marker-icon-2x.png';
const iconUrl = 'assets/marker-icon.png';
const shadowUrl = 'assets/marker-shadow.png';
const iconDefault = L.icon({
  iconRetinaUrl,
  iconUrl,
  shadowUrl,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});
L.Marker.prototype.options.icon = iconDefault;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
})
export class MapComponent implements OnInit, AfterViewInit {
  constructor(
    private mapService: MapService,
    private compumatService: CompumatService,
    private markerService: MarkerService,
    private dialog: MatDialog,
    private router: Router
  ) {}


  public map: L.Map;
  mapData: MapData;
  markerservice: MarkerService;

  compumats: Compumat[] = [
    {
      icon: null,
      id: '01',
      latitude: 55.31404492554651,
      longitude: 10.787099052273176,
      name: 'Gate01',
      status: 'ok',
      type: CompumatType.GATE,
    },
  ];

  returnCurrentCompumat(): Compumat {
    let currentId = this.markerService.getCurrentCompumat();
    return currentId;
  }


  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
      this.mapService.getMapData(1).subscribe((mapData) => {
        this.mapData = mapData;
      this.initMap();
      this.markerService.loadMarkers(this.map);
      });
  }

  initMap(): void {
    this.map = L.map('map', {
      center: [this.mapData.latitude, this.mapData.longitude],
      zoom: 18,
    });

    let bounds = this.map.getBounds();

    const tiles = L.tileLayer(
      'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
      {
        maxZoom: 18,
        minZoom: 16,
        attribution:
          '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      }
    );
    this.map.setMaxBounds(bounds);
    tiles.addTo(this.map);
    this.map.setZoom(17);
  }

  openMoreDetails(): void {
    const dialogRef = this.dialog.open(MapDialogComponent, {
      width: '700px',
      height: '700px',
      data: {data: this.returnCurrentCompumat()},
      });
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.compumats = result;
    });
}

}
