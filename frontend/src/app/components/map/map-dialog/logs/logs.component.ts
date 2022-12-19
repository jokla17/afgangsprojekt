import { Component, Input, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { Compumat } from 'src/app/components/compumat/compumat';
import { CompumatService } from 'src/app/components/compumat/compumat.service';
import { LogEvent } from 'src/app/components/eventlist/event/LogEvent';
import { LogEntry } from './LogEntry';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.scss'],
})
export class LogsComponent implements OnInit {
  @Input() compumat: Compumat;
  public logs: LogEntry[] = [];

  columnsToDisplay = ['Index', 'Date', 'Time', 'StationNo', 'Message'];

  constructor(private compumatService: CompumatService) {}

  ngOnInit(): void {
    console.log("Compumat: " + this.compumat.stationNo);
    this.compumatService.getCompumatLogs(this.compumat.stationNo).pipe(take(1)).subscribe(
      (logs) => {
        this.logs = [...logs];
        console.log(this.logs);
      },
      (error) => {
        console.log('Error caught in logs-component. Message: \n' + error.toString());
      });
  }
}
