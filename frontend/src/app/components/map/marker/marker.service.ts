/// <reference path="../../../../../typings/index.d.ts" />
import { Injectable } from '@angular/core';
import { Compumat } from '../../compumat/compumat';
import * as L from 'leaflet';
import { CompumatType } from '../../compumat/compumatType.enum';
import { Icons } from '../../icon/icons.enum';

@Injectable({
  providedIn: 'root',
})
export class MarkerService {
  markerStyle = `stroke: blue`;

  iconColors = {
    ok: 'green',
    warning: 'yellow',
    error: 'red',
    offline: 'gray',
  };

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
    {
      icon: null,
      id: '02',
      latitude: 55.31504492554651,
      longitude: 10.787099052273176,
      name: 'VendingMachine01',
      status: 'error',
      type: CompumatType.VENDINGMACHINE,
    },
    {
      icon: null,
      id: '03',
      latitude: 55.31304492554651,
      longitude: 10.787099052273176,
      name: 'Gate02',
      status: 'offline',
      type: CompumatType.GATE,
    },
  ];

  constructor() {}

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

  makeCustomCircleMarkers(map: L.Map): void {
    for (let c of this.compumats) {
      let iconSettings = {
        mapIconUrl: `
          <svg
            xmlns="http://www.w3.org/2000/svg"
            style="height: 4rem; width: 4rem; transform:scale(0.7)">
            <use
              xlink:href="${Icons[CompumatType[c.type]]}"
              style="stroke:${this.iconColors[c.status]}"
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

      //let popup = L.popup().setContent(JSON.stringify(c, null, "\n"));
      let compumatText = JSON.stringify(c, null, "<br>");
      let popup = L.popup().setContent(
        `<div>${compumatText}</div>`
      );

      let icon = L.marker([c.latitude, c.longitude], {
        icon: svgIcon,
        clickable: true,
      }).bindPopup(popup, {
        className: 'customPopup'
      });

      icon.addTo(map);
    }
  }
}
