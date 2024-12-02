
import {AppConfig} from './app.config.interface';
import {InjectionToken} from '@angular/core';
import {environment} from '../../environments/environment';

export const APP_SERVICE_CONFIG = new InjectionToken<AppConfig>('app.config')

export const APP_CONFIG ={
  apiEndpoint:environment.apiEndpoint
}
