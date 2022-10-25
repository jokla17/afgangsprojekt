import { Component, OnInit } from '@angular/core';
import 'ol/ol.css';
import Map from 'ol/Map';
import View from 'ol/View';
import { OSM } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import { MapData } from './MapData';
import { MapService } from './map.service';
import { useGeographic } from 'ol/proj';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {

  constructor(private mapService:MapService) { }

  public map!: Map;
  mapData: MapData;

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
    })
  }

 initMap(): void{
  useGeographic();
  this.map = new Map({
    layers: [
      new TileLayer({
        source: new OSM(),
      }),
    ],
    target: 'map',
    view: new View({
      center: [this.mapData.longitude, this.mapData.latitude],
      zoom: 18,
    }),
  });
  this.map.on("click", () => console.log(this.map.getView().calculateExtent(this.map.getSize())))
  this.map.setView(new View({
    center: [this.mapData.longitude, this.mapData.latitude],
    extent: this.map.getView().calculateExtent(this.map.getSize()),
    zoom: 18,
    minZoom: 18,
    maxZoom: 19
  }))
 }
}
