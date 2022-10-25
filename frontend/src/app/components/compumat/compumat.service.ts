import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Compumat } from './compumat';

@Injectable({
  providedIn: 'root'
})
export class CompumatService {

constructor(
  private http:HttpClient
  ) { }

  // Get individual compumat path {id}/Compumat

  getIcon(id: string): Observable<Compumat> {
    return this.http.get<Compumat>(`https://localhost:7049/${id}/Compumat`);
  }

  // Get all compumats path /Compumats
  getIcons(): Observable<Compumat[]> {
    return this.http.get<Compumat[]>(`https://localhost:7049/Compumats`);
  }
}
