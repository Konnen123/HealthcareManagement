import { Routes } from '@angular/router';
import {AuthenticationGuard} from './guards/authentication.guard';

export const routes: Routes = [
  {
    path: 'appointments',
    canActivateChild: [AuthenticationGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./components/appointment/appointment-list/appointment-list.component').then(m=>m.AppointmentListComponent)
      },
      {
        path: 'create',
        loadComponent: () => import('./components/appointment/appointment-create/appointment-create.component').then(m=>m.AppointmentCreateComponent)
      },
      {
        path: ':id',
        loadComponent: () => import('./components/appointment/appointment-detail/appointment-detail.component').then(m=>m.AppointmentDetailComponent)
      },
      {
        path: 'update/:id',
        loadComponent: () => import('./components/appointment/appointment-update/appointment-update.component').then(m=>m.AppointmentUpdateComponent)
      }
    ],
  },
  {
    path: 'access-denied',
    loadComponent: () => import('./components/access-denied/access-denied.component').then(m=>m.AccessDeniedComponent),
  },
  {
    path: '**',
    loadComponent: () => import('./components/not-found/not-found.component').then(m=>m.NotFoundComponent),
  }
];
