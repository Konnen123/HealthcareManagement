import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'appointments',
    loadComponent: () =>import('./components/appointment-list/appointment-list.component').then(m=>m.AppointmentListComponent)
  }
];
