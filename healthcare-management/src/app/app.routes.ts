import { Routes } from '@angular/router';
import { AuthenticationGuard } from './guards/authentication/authentication.guard';
import { RoleGuard } from './guards/role/role.guard';

export const routes: Routes = [
  {
    path: 'appointments',
    canActivateChild: [AuthenticationGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./components/appointment/appointment-list/appointment-list.component').then(m => m.AppointmentListComponent),
        canActivate: [RoleGuard],
        data: { expectedRole: ['DOCTOR'] }
      },
      {
        path: 'create',
        loadComponent: () => import('./components/appointment/appointment-create/appointment-create.component').then(m => m.AppointmentCreateComponent),
        canActivate: [RoleGuard],
        data: { expectedRole: ['PATIENT'] }
      },
      {
        path: ':id',
        loadComponent: () => import('./components/appointment/appointment-detail/appointment-detail.component').then(m => m.AppointmentDetailComponent)
      },
      {
        path: 'update/:id',
        loadComponent: () => import('./components/appointment/appointment-update/appointment-update.component').then(m => m.AppointmentUpdateComponent),
        canActivate: [RoleGuard],
        data: { expectedRole: ['DOCTOR'] }
      }
    ],
  },
  {
    path: 'symptom-checker',
    loadComponent: () => import('./components/symptom-checker/symptom-checker.component').then(m => m.SymptomCheckerComponent),
    canActivate: [AuthenticationGuard, RoleGuard],
    data: { expectedRole: ['PATIENT', "DOCTOR"] }
  },
  {
    path: '',
    loadComponent: () => import('./components/landing-page/landing-page.component').then(m => m.LandingPageComponent),
    data: { isNavbarHidden: true },
  },
  {
    path: 'home',
    loadComponent: () => import('./components/home-page/home-page.component').then(m => m.HomePageComponent),
    canActivate: [AuthenticationGuard],
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent),
    data: { isNavbarHidden: true }
  },
  {
    path: 'signup',
    loadComponent: () => import('./components/signup/signup.component').then(m => m.SignupComponent),
    data: { isNavbarHidden: true }
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./components/forgot-password/forgot-password-1/forgot-password-1.component').then(m => m.ForgotPassword1Component),
    data: { isNavbarHidden: true }
  },
  {
    path: 'verify-email',
    loadComponent: () => import('./components/verify-email/verify-email.component').then(m => m.VerifyEmailComponent),
    data: { isNavbarHidden: true }
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./components/forgot-password/forgot-password-2/forgot-password-2.component').then(m => m.ForgotPassword2Component),
    data: { isNavbarHidden: true }
  },
  {
    path: 'access-denied',
    loadComponent: () => import('./components/access-denied/access-denied.component').then(m => m.AccessDeniedComponent),
    data: { isNavbarHidden: true }
  },
  {
    path: '**',
    loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent),
    data: { isNavbarHidden: true }
  },

];
