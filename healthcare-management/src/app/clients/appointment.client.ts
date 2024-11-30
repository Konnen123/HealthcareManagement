import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AppConfig} from '../app-config/app.config.interface';
import {APP_SERVICE_CONFIG} from '../app-config/app.config';
import {Observable} from 'rxjs';
import {Appointment} from '../models/appointment.model';

@Injectable({
  providedIn: 'root',
})
export class AppointmentClient
{
  private readonly baseUrl: string;
  constructor(@Inject(APP_SERVICE_CONFIG) private config: AppConfig, private http: HttpClient)
  {
    this.baseUrl = this.config.apiEndpoint + '/v1/Appointments';
  }

  public getAllAppointments(): Observable<Appointment[]>
  {
    return this.http.get<Appointment[]>(this.baseUrl);
  }
}