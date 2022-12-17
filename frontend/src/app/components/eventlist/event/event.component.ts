import { Component, Input, OnInit } from '@angular/core';
import { MarkerService } from '../../map/marker/marker.service';
import { LogEvent } from './LogEvent';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss'],
})
export class EventComponent implements OnInit {
  @Input() logEvent: LogEvent;
  @Input() timestamp: string;
  @Input() threatLevel: string;

  constructor(
    private markerService: MarkerService,
  ) {}

  ngOnInit(): void {}

  onMouseEnter(stationNo: string) {
    console.log('Mouse entered: ' + stationNo);
    this.markerService.highlightMarker(stationNo);
  }

  onMouseLeave(stationNo: string) {
    console.log('Mouse left: ' + stationNo);
    this.markerService.undoHighlightMarker(stationNo);
  }
}
