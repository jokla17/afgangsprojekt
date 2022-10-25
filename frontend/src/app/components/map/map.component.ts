import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MapData } from './MapData';
import { MapService } from './map.service';
import { Compumat } from '../compumat/compumat';
import { CompumatType } from '../compumat/compumatType.enum';
import * as L from 'leaflet';
import { MarkerService } from './marker/marker.service';

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
    private markerService: MarkerService,
  ) {}


  public map: L.Map;
  mapData: MapData;

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

  ngOnInit(): void {
    // this.mapService.getMapData(1).subscribe(data => {
    //   this.mapData = data;
    // })
  }

  ngAfterViewInit(): void {
      this.mapService.getMapData(1).subscribe((mapData) => {
        this.mapData = mapData;
        console.log(this.mapData);
      this.initMap();
      this.markerService.makeCustomCircleMarkers(this.map);
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

}
