import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MapData } from './MapData';

@Injectable({
  providedIn: 'root'
})
export class MapService {

constructor(private http:HttpClient) { }

getMapData(): Observable<MapData>{
  return this.http.get<MapData>("https://localhost:7049/Map");
}
}
