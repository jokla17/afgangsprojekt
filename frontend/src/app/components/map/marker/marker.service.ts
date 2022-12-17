/// <reference path="../../../../../typings/index.d.ts" />
import { Injectable, Output } from '@angular/core';
import { Compumat } from '../../compumat/compumat';
import * as L from 'leaflet';
import { CompumatType } from '../../compumat/compumatType.enum';
import { Icons } from '../../icon/icons.enum';
import { CompumatService } from '../../compumat/compumat.service';
import { MapComponent } from '../map.component';
import { MapService } from '../map.service';

@Injectable({
  providedIn: 'root',
})
export class MarkerService {
  markerStyle = `stroke: blue`;

  public selectedCompumat: Compumat = null;

  iconColors = {
    ok: 'green',
    warning: 'yellow',
    error: 'red',
    offline: 'gray',
  };

  public compumats: Compumat[] = [];
  private markers: L.Marker[] = [];

  constructor(
    private compumatService: CompumatService,
    private mapService: MapService
  ) {
    this.mapService.selectedCompumat.subscribe(compumat => {
      this.selectedCompumat = compumat;
    })
  }

  makeCompumatMarkers(map: L.Map): void {
    for (let c of this.compumats) {
      let lat = c.latitude;
      let lng = c.longitude;
      let marker = L.marker([lat, lng]);

      marker.addTo(map);
    }
  }

  makeCompumatCircleMarkers(map: L.Map): void {
    for (let c of this.compumats) {
      let lat = c.latitude;
      let lng = c.longitude;
      let circle = L.circleMarker([lat, lng]).on('click', () => {
        console.log(c);
      });

      circle.addTo(map);
    }
  }

  loadMarkers(map: L.Map, compumats: Compumat[]): void {
    this.compumats = compumats;
    this.makeCustomCircleMarkers(map);
  }

  highlightMarker(stationNo: string){
    console.log(stationNo);
    let compumatIndex = this.compumats.findIndex(c => c.stationNo == stationNo);
    console.log(compumatIndex);
    if(compumatIndex > -1) this.markers[compumatIndex].fire("highlight");
  }

  undoHighlightMarker(stationNo: string){
    let compumatIndex = this.compumats.findIndex((c) => c.stationNo === stationNo);
    if(compumatIndex > -1) this.markers[compumatIndex].fire('undoHighlight');
  }

  makeCustomCircleMarkers(map: L.Map): void {
    //TODO: remove trim from c.status
    this.markers.forEach(marker => {
      map.removeLayer(marker);
    });
    this.markers = [];

    for (let c of this.compumats) {
      let iconSettings = {
        mapIconUrl: `
          <svg
            xmlns="http://www.w3.org/2000/svg"
            style="height: 4rem; width: 4rem; transform:scale(0.7)">
            <use
              xlink:href="${Icons[CompumatType[c.type]]}"
              style="stroke:${this.iconColors[c.status.trim()]}"
            ></use>
          </svg>
        `,
      };

      let highlightedIconSettings = {
        mapIconUrl: `
          <svg
            xmlns="http://www.w3.org/2000/svg"
            style="
            margin: -1rem;
            height: 4rem;
            width: 4rem;
            transform:scale(0.7);
            border: 1rem solid yellow;">
            <use
              xlink:href="${Icons[CompumatType[c.type]]}"
              style="stroke:${this.iconColors[c.status.trim()]}"
            ></use>
          </svg>
        `,
      };

      let svgIcon = L.divIcon({
        html: L.Util.template(iconSettings.mapIconUrl, iconSettings),
        className: '',
        iconSize: [25, 25],
        iconAnchor: [30, 30],
        popupAnchor: [65, 36],
      });

      let highlightedSvgIcon = L.divIcon({
        html: L.Util.template(
          highlightedIconSettings.mapIconUrl,
          highlightedIconSettings
        ),
        className: '',
        iconSize: [25, 25],
        iconAnchor: [30, 30],
        popupAnchor: [65, 36],
      });

      let popup = L.popup().setContent(`
        <div>
          <h3>${c.name}</h3>
          <p>Id: ${c.id}</p>
          <p>StationNo: ${c.stationNo}</p>
          <p>Type: ${CompumatType[c.type]}</p>
          <p>Status: ${c.status}</p>
          <button (click)="testButton()"> Click for more details</button>
        </div>
      `);

      let icon = L.marker([c.latitude, c.longitude], {
        icon: svgIcon,
        clickable: true,
      }).bindPopup(popup, {
        className: 'customPopup',
        offset: L.point(50,0)
      });

      let highlightedIcon = L.marker([c.latitude, c.longitude], {
        icon: highlightedSvgIcon,
        clickable: true,
      }).bindPopup(popup, {
        className: 'customPopup',
      });

      icon.on('click', () => {
        this.mapService.setCurrentCompumat(c);
      });

      icon.on('highlight', () => {
        console.log("HIGHLIGHTED");
        icon.setIcon(highlightedSvgIcon);
      });

      icon.on('undoHighlight', () => {
        console.log('UNDO HIGHLIGHT');
        icon.setIcon(svgIcon);
      })

      this.markers.push(icon);
      icon.addTo(map);
    }
  }

  getCurrentCompumat(): Compumat {
    return this.selectedCompumat;
  }

  resetCurrentCompumat(): void {
    console.log("reset selected compumat")
    this.mapService.setCurrentCompumat(null);
  }
}
