import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { CompumatType } from 'src/app/components/compumat/compumatType.enum';
import { Command } from './command';

@Injectable({
  providedIn: 'root',
})
export class CommandService {
  private _commandData: BehaviorSubject<Command[]> = new BehaviorSubject([]);

  public commandData: Observable<Command[]> = this._commandData.asObservable();

  constructor(private http: HttpClient) {
    this.loadInitialData();
  }

  loadInitialData() {
    this.getCommands("admin_22").subscribe(
      (resp) => {
        let commands = resp;
        this._commandData.next(commands);
      },
      (err) => console.log('Error retrieving Commands')
    );
  }

  getCommands(user: string): Observable<Command[]> {
    return this.http.get<Command[]>(`https://localhost:7049/Command/GetCommands/${user}`);
  }

  localAdd(command: Command) {
    let currentCommands: Command[];
    this.commandData.pipe(take(1)).subscribe((commands) => {
      currentCommands = commands;
      currentCommands.push(command);
      this._commandData.next(currentCommands);
    });
  }

  localChange(command: Command) {
    let currentCommands: Command[];
    this.commandData.pipe(take(1)).subscribe((commands) => {
      currentCommands = commands;
      let compumatIndex = currentCommands.indexOf(
        currentCommands.find((element) => element.commandString === command.commandString)
      );
      currentCommands[compumatIndex] = command;
      this._commandData.next(currentCommands);
    });
  }

  localRemove(commandString: string) {
    let currentCommands: Command[];
    this.commandData.pipe(take(1)).subscribe((commands) => {
      currentCommands = commands;
      let compumatIndex = currentCommands.indexOf(
        currentCommands.find((element) => element.commandString == commandString)
      );
      if (compumatIndex > -1) currentCommands.splice(compumatIndex, 1);
      this._commandData.next(currentCommands);
    });
  }
}
