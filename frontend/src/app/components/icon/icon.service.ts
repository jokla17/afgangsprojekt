import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompumatType } from '../compumat/compumatType.enum';
import { Icon } from './icon';
import { Icons } from './icons.enum';

@Injectable({
  providedIn: 'root'
})
export class IconService {

  svgIcon: Icon;

constructor(
  private http: HttpClient,
) { }

  getIcon(compumatType: CompumatType): Icon {
    let icon: Icon = {
      type: compumatType,
      iconPath: Icons[CompumatType[compumatType]]
    }
    return icon;
  }

  // Get all compumats path /Compumats
  getIcons(): Icon[] {
    let icons: Icon[] = [];
    for(let compumatType in CompumatType){
      let icon: Icon = {
        type: CompumatType[CompumatType[compumatType]],
        iconPath: Icons[CompumatType[compumatType]]
      }
      icons.push(icon);
    }
    return icons;
  }
}
