import { AfterViewInit, Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
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
import { SignalrService } from '../services/signalr.service';
import { HttpClient } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { EventService } from '../eventlist/event/event.service';

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
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MapComponent implements OnInit, AfterViewInit {
  constructor(
    private mapService: MapService,
    private compumatService: CompumatService,
    private markerService: MarkerService,
    private dialog: MatDialog,
    private router: Router,
    private signalRService: SignalrService,
    private http: HttpClient,
    private cd: ChangeDetectorRef,
    private eventService: EventService
  ) {}

  selectedCompumatTypes = new FormControl<string[]>([]);
  compumatTypeList: string[] = Object.keys(CompumatType).filter((t) => {
    return isNaN(Number(t));
  });
  public map: L.Map;
  public mapData: MapData;
  public markerservice: MarkerService;

  compumats: Compumat[] = [];
  public currentCompumat: Compumat = null;

  getCurrentCompumat(): Compumat {
    console.log("getting")
    return this.markerService.getCurrentCompumat();
  }

  hasSelectedCompumat(): boolean {
    if (this.markerService.getCurrentCompumat() !== null) return true;
    else return false;
  }

  ngOnInit(): void {
    this.compumatService.compumatData.subscribe((compumats) => {
      this.compumats = compumats;
      this.cd.markForCheck();
    });
    this.mapService.selectedCompumat.subscribe(compumat => {
      this.currentCompumat = compumat;
      this.cd.markForCheck();
    });
    this.signalRService.startConnection();
    this.signalRService.onDataUpdate(this.update.bind(this));
  }

  update() {
    this.markerService.loadMarkers(this.map, this.compumats);
  }

  ngAfterViewInit(): void {
    this.mapService.getMapData(1).subscribe((mapData) => {
      this.mapData = mapData;
      this.initMap();
      this.markerService.loadMarkers(this.map, this.compumats);
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
    this.map.on('click', () => {
      this.markerService.resetCurrentCompumat();
    });
  }

  openMoreDetails(): void {
    const dialogRef = this.dialog.open(MapDialogComponent, {
      width: '700px',
      height: '700px',
      data: { data: this.getCurrentCompumat() },
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
      // TODO: fix, so that any changes are actually sent to the API
      this.compumats = result;
    });
  }

  filterChange(filters: string[]) {
    this.selectedCompumatTypes.setValue(filters);
    this.eventService.filterChange(filters);
    let filteredCompumats: Compumat[] = this.compumats;
    if (filters.length > 0) {
      filteredCompumats = this.compumats.filter((c) =>
        filters.includes(CompumatType[c.type])
      );
    }
    this.markerService.loadMarkers(this.map, filteredCompumats);
    this.markerService.makeCustomCircleMarkers(this.map);
  }

  clearFilter() {
    this.selectedCompumatTypes.setValue([]);
    this.filterChange(this.selectedCompumatTypes.value);
    this.eventService.filterChange(this.selectedCompumatTypes.value);
  }
}
