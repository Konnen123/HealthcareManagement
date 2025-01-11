import {Inject, Injectable} from '@angular/core';
import {APP_SERVICE_CONFIG} from '../app-config/app.config';
import {AppConfig} from '../app-config/app.config.interface';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class AuthenticationClient {
  private readonly baseUrl: string;
  constructor(@Inject(APP_SERVICE_CONFIG) readonly config: AppConfig, readonly http: HttpClient)
  {
    this.baseUrl = this.config.apiEndpoint + '/v1/Auth'
  }

  public register(userData: any) : Observable<any> {
    return this.http.post(`${this.baseUrl}/Register`, userData);
  }

  public login(userData: any) : Observable<any> {
    return this.http.post(`${this.baseUrl}/Login`, userData);
  }

  public refreshToken(refreshToken: string) : Observable<any> {
    return this.http.post(`${this.baseUrl}/refresh`, {refreshToken});
  }
}
