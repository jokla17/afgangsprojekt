import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MapData } from './MapData';

@Injectable({
  providedIn: 'root'
})
export class MapService {

constructor(private http:HttpClient) { }

getMapData(id:number): Observable<MapData>{
  return this.http.get<MapData>(`https://localhost:7049/Map/ReadOne?id=${id}`);
}

getAllMapData(): Observable<MapData[]>{
  return this.http.get<MapData[]>('https://localhost:7049/Map/ReadAll');
}

createMapData(mapData:MapData): Observable<MapData>{
  return this.http.post<MapData>('https://localhost:7049/Map/Create', mapData);
}

updateMapData(mapData:MapData): Observable<MapData>{
  return this.http.put<MapData>('https://localhost:7049/Map/Update', mapData);
}

deleteMapData(id:number): Observable<MapData>{
  return this.http.delete<MapData>(`https://localhost:7049/Map/Delete?id=${id}`);
}


}
