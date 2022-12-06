import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommandComponent } from './components/map/map-dialog/command/command.component';
import { ConfigurationComponent } from './components/map/map-dialog/configuration/configuration.component';
import { InfoComponent } from './components/map/map-dialog/info/info.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot([
    { path: '', pathMatch: 'full', redirectTo: '' },
    { path: 'info', component: InfoComponent },
    { path: 'commands', component: CommandComponent },
    { path: 'configuration', component: ConfigurationComponent },
  ])],
  exports: [RouterModule]
})
export class AppRoutingModule { }
