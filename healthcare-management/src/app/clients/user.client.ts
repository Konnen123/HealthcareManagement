import {Inject, Injectable} from '@angular/core';
import {APP_SERVICE_CONFIG} from '../app-config/app.config';
import {AppConfig} from '../app-config/app.config.interface';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class UserClient{
  private readonly baseUrl: string;
  constructor(@Inject(APP_SERVICE_CONFIG) private config: AppConfig, private http: HttpClient)
  {
    this.baseUrl = this.config.apiEndpoint + '/v1/'
  }

  public register() : Observable<any> {
    return this.http.post(this.baseUrl);
  }

}
