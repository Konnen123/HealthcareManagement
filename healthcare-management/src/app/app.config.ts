import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import {provideHttpClient, withFetch, withInterceptors} from '@angular/common/http';
import {APP_CONFIG, APP_SERVICE_CONFIG} from './app-config/app.config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {MAT_FORM_FIELD_DEFAULT_OPTIONS} from '@angular/material/form-field';
import { provideNativeDateAdapter } from '@angular/material/core';
import {authenticationInterceptor} from './interceptor/authentication.interceptor';
import {JWT_OPTIONS, JwtHelperService} from '@auth0/angular-jwt';
import { FormsModule } from '@angular/forms';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withFetch()),
    provideNativeDateAdapter(),
    {provide: APP_SERVICE_CONFIG, useValue: APP_CONFIG},
    provideAnimationsAsync(),
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: {appearance: 'outline', subscriptSizing: 'dynamic'}
    },
    provideHttpClient(withFetch(), withInterceptors([authenticationInterceptor])),
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS }, JwtHelperService,
    FormsModule
  ]
};
