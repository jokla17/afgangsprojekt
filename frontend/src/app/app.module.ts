import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MapComponent } from './components/map/map.component';
import { HttpClientModule } from '@angular/common/http';
import { CompumatComponent } from './components/compumat/compumat.component';
import { IconComponent } from './components/icon/icon.component';
import { MapDialogComponent } from './components/map/map-dialog/map-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { CommandComponent } from './components/map/map-dialog/command/command.component';
import { InfoComponent } from './components/map/map-dialog/info/info.component';
import { ConfigurationComponent } from './components/map/map-dialog/configuration/configuration.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTableModule} from '@angular/material/table';
import {MatInputModule} from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { EventlistComponent } from './components/eventlist/eventlist.component';
import { EventComponent } from './components/eventlist/event/event.component';
import { MatIconModule } from '@angular/material/icon';
import { LogsComponent } from './components/map/map-dialog/logs/logs.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle'
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
  declarations: [
    AppComponent,
    MapComponent,
    CompumatComponent,
    IconComponent,
    MapDialogComponent,
    CommandComponent,
    InfoComponent,
    ConfigurationComponent,
    EventlistComponent,
    EventComponent,
    LogsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatDialogModule,
    MatTabsModule,
    MatFormFieldModule,
    MatTableModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatIconModule,
    MatButtonToggleModule,
    MatTooltipModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
