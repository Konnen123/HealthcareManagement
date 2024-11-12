import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AppConfig} from '../app-config/app.config.interface';
import {APP_SERVICE_CONFIG} from '../app-config/app.config';
import {Observable} from 'rxjs';
import {Appointment} from '../models/appointment.model';

@Injectable({
  providedIn: 'root',
})
export class AppointmentClient {
  private baseUrl: string = '/api/v1/Appointments';
  constructor(@Inject(APP_SERVICE_CONFIG) private config: AppConfig, private http: HttpClient) {}

  public getAllAppointments(): Observable<Appointment[]>
  {
    return this.http.get<Appointment[]>(this.baseUrl);
  }
}
