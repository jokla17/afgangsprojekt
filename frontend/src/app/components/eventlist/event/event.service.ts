import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Compumat } from '../../compumat/compumat';
import { LogEvent } from './LogEvent';
import { CompumatService } from '../../compumat/compumat.service';
import { CompumatType } from '../../compumat/compumatType.enum';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  private _logEvent: BehaviorSubject<LogEvent[]> = new BehaviorSubject([]);
  public logEvents: Observable<LogEvent[]> = this._logEvent.asObservable();

  private unfilteredEvents: LogEvent[];

  private _filter: BehaviorSubject<string[]> = new BehaviorSubject([]);
  public filter: Observable<string[]> = this._filter.asObservable();

  constructor(private compumatService: CompumatService) {}

  parseAndAddEvent(logEvent: string, threatLevel: string): void {
    let splitMsg: string[] = logEvent.split(';');
    let stationNo = splitMsg[2];
    this.compumatService.compumatData.pipe(take(1)).subscribe((compumats) => {
      let compumatMatch = compumats.find((c) => {
        if (c.stationNo === stationNo) {
          return c;
        } else return undefined;
      });
      let unknownCompumat: Compumat = {
        icon: null,
        id: null,
        latitude: null,
        longitude: null,
        name: 'Unknown',
        stationNo: '?',
        status: 'Unknown',
        type: null,
      };
      let newEvent: LogEvent = {
        compumat: compumatMatch !== undefined ? compumatMatch : unknownCompumat,
        message: splitMsg[4],
        timestamp: splitMsg[1],
        threatLevel: compumatMatch !== undefined ? threatLevel : 'warn',
      };
      this.localAdd(newEvent);
    });
  }

  localAdd(event: LogEvent) {
    let currentEvents: LogEvent[];
    this.logEvents.pipe(take(1)).subscribe((events) => {
      currentEvents = events;
      currentEvents.push(event);
      this._logEvent.next(currentEvents);
    });
  }

  filterChange(filter: string[]) {
    this._filter.next(filter);
  }
}
