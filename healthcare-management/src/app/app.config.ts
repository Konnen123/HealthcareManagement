import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import {provideHttpClient, withFetch} from '@angular/common/http';
import {APP_CONFIG, APP_SERVICE_CONFIG} from './app-config/app.config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {MAT_FORM_FIELD_DEFAULT_OPTIONS} from '@angular/material/form-field';
import { provideNativeDateAdapter } from '@angular/material/core';
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
    FormsModule
  ]
};
