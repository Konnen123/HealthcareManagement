import { Routes } from '@angular/router';
import {AppointmentCreateComponent} from './components/appointment-create/appointment-create.component';

export const routes: Routes = [
  {
    path: 'appointments',
    loadComponent: () =>import('./components/appointment-list/appointment-list.component').then(m=>m.AppointmentListComponent)
  },
  {
    path: 'appointments/create',
    loadComponent: () =>import('./components/appointment-create/appointment-create.component').then(m=>m.AppointmentCreateComponent)
  }
];
