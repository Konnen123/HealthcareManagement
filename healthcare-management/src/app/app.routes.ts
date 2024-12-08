import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'appointments',
    loadComponent: () =>import('./components/appointment-list/appointment-list.component').then(m=>m.AppointmentListComponent)
  },
  {
    path: 'appointments/create',
    loadComponent: () =>import('./components/appointment-create/appointment-create.component').then(m=>m.AppointmentCreateComponent)
  },
  {
    path: 'appointments/:id',
    loadComponent: () =>import('./components/appointment-detail/appointment-detail.component').then(m=>m.AppointmentDetailComponent)
  },
  {
    path: 'appointments/update/:id',
    loadComponent: () =>import('./components/appointment-update/appointment-update.component').then(m=>m.AppointmentUpdateComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'signup',
    loadComponent: () => import('./components/signup/signup.component').then(m => m.SignupComponent)
  }
];
