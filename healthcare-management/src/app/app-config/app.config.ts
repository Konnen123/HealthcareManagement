import {environment} from '../../environments/environment';
import {AppConfig} from './app.config.interface';
import {InjectionToken} from '@angular/core';

export const APP_SERVICE_CONFIG = new InjectionToken<AppConfig>('app.config')

export const APP_CONFIG ={
  apiEndpoint:environment.apiEndpoint
}
