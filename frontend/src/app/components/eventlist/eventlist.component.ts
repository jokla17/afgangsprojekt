import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CompumatType } from '../compumat/compumatType.enum';
import { EventService } from './event/event.service';
import { LogEvent } from './event/LogEvent';

@Component({
  selector: 'app-eventlist',
  templateUrl: './eventlist.component.html',
  styleUrls: ['./eventlist.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EventlistComponent implements OnInit {
  public events: LogEvent[] = [];
  public eventFilter: string[] = [];
  public compumatTypesEnum = CompumatType;
  public logModeControl = new FormControl('Live');

  constructor(
    private eventService: EventService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.eventService.logEvents.subscribe((events) => {
      this.events = events;
      this.cd.markForCheck();
    });

    this.eventService.filter.subscribe((filter) => {
      this.eventFilter = filter;
      this.cd.markForCheck();
    });
  }

  isHidden(event: LogEvent): boolean {
    return this.eventFilter.length > 0 && this.eventFilter.includes(this.compumatTypesEnum[event.compumat.type]) === false;
  }

  logModeChange(event: string){
    if (event === "Live"){

    } else if (event === "History"){

    }
  }
}
