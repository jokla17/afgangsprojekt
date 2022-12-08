import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { Compumat } from '../compumat/compumat';
import { CompumatService } from '../compumat/compumat.service';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  public data;
  private hubConnection: signalR.HubConnection;

  constructor(
    private compumatService: CompumatService,
  ) {}

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7049/compumatHub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection established'))
      .catch((err) => console.log('Error while starting connection: ' + err));

    this.addTransferCompumatDataListener();
  }

  private addTransferCompumatDataListener() {
    this.hubConnection.on('AddCompumat', (data: Compumat) => {
      this.compumatService.localAdd(data);
      this.updateData();
    });

    this.hubConnection.on('RemoveCompumat', (data: string) => {
      this.compumatService.localRemove(data);
      this.updateData();
    });


    this.hubConnection.on('ChangeCompumat', (data: Compumat) => {
      this.compumatService.localChange(data);
      this.updateData();
    });
  }

  private updateData!: () => void;
  onDataUpdate(fn: () => void) {
    this.updateData = fn;
  }
}
