import {Inject, Injectable} from '@angular/core';
import {APP_SERVICE_CONFIG} from '../app-config/app.config';
import {AppConfig} from '../app-config/app.config.interface';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {DoctorDto} from '../shared/dtos/doctor.dto';

@Injectable({
  providedIn: 'root',
})

export class UserClient
{
  private readonly baseUrl: string;
  constructor(@Inject(APP_SERVICE_CONFIG) readonly config: AppConfig, readonly http: HttpClient)
  {
    this.baseUrl = this.config.apiEndpoint + '/v1/Users'
  }

  public getAllDoctors(): Observable<DoctorDto[]>
  {
    return this.http.get<DoctorDto[]>(`${this.baseUrl}/Doctors`);
  }


}
