import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MapComponent } from './components/map/map.component';
import { HttpClientModule } from '@angular/common/http';
import { CompumatComponent } from './components/compumat/compumat.component';
import { IconComponent } from './components/icon/icon.component';
import { BoomIconComponent } from './components/icon/boom-icon/boom-icon.component';
import { VendingIconComponent } from './components/icon/vending-icon/vending-icon.component';

@NgModule({
  declarations: [
    AppComponent,
    MapComponent,
    CompumatComponent,
    IconComponent,
    BoomIconComponent,
    VendingIconComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
