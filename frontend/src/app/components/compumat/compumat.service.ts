import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { async, BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Compumat } from './compumat';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CompumatService {
  private _compumatData: BehaviorSubject<Compumat[]> = new BehaviorSubject([]);

  public compumatData: Observable<Compumat[]> = this._compumatData.asObservable();

constructor(
  private http:HttpClient
  ) {
    this.loadInitialData();
  }

  loadInitialData() {
    this.getCompumats()
      .subscribe(
        resp => {
          let compumats = resp;
          this._compumatData.next(compumats);
        },
        err => console.log("Error retrieving Compumats")
      );
  }

  getCompumat(id: string): Observable<Compumat> {
    return this.http.get<Compumat>(`https://localhost:7049/${id}/Compumat`);
  }

  getCompumats(): Observable<Compumat[]> {
    return this.http.get<Compumat[]>(`https://localhost:7049/Compumat/ReadAll`);
  }

  createCompumat(compumat: Compumat): Observable<Compumat> {
    return this.http.post<Compumat>(`https://localhost:7049/Create`, compumat);
  }

  updateCompumat(compumat: Compumat): Observable<Compumat> {
    return this.http.put<Compumat>(`https://localhost:7049/Update`, compumat);
  }

  deleteCompumat(id: string): Observable<Compumat> {
    return this.http.delete<Compumat>(`https://localhost:7049/${id}/Delete`);
  }

  localAdd(compumat: Compumat) {
    let currentCompumats: Compumat[];
    this.compumatData.pipe(take(1)).subscribe((compumats) => {
      currentCompumats = compumats;
      currentCompumats.push(compumat);
      this._compumatData.next(currentCompumats);
    });
  }

  localChange(compumat: Compumat){
    let currentCompumats: Compumat[];
    this.compumatData.pipe(take(1)).subscribe((compumats) => {
      currentCompumats = compumats;
      let compumatIndex = currentCompumats.indexOf(
        currentCompumats.find((element) => element.id === compumat.id)
      );
      currentCompumats[compumatIndex] = compumat;
      this._compumatData.next(currentCompumats);
    });
  }

  localRemove(compumatId: string){
    let currentCompumats: Compumat[];
    this.compumatData.pipe(take(1)).subscribe((compumats) => {
      currentCompumats = compumats;
      let compumatIndex = currentCompumats.indexOf(currentCompumats.find((element) => element.id == compumatId));
      if (compumatIndex > -1) currentCompumats.splice(compumatIndex, 1);
      this._compumatData.next(currentCompumats);
    });
  }

}
