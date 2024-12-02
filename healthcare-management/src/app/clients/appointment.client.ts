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

  public createAppointment(appointment: Appointment): Observable<any>
  {
    return this.http.post<Appointment>(this.baseUrl, appointment);
  }

  public getAppointmentById(id: string): Observable<Appointment>
  {
    return this.http.get<Appointment>(`${this.baseUrl}/${id}`);
  }

  public updateAppointment(appointment: Appointment): Observable<any>
  {
    return this.http.put(`${this.baseUrl}/${appointment.id}`, appointment);
  }

  public deleteAppointment(id: string): Observable<any>
  {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  public getAppointmentsPaginated(top: number, skip:number) {
    return this.http.get<Appointment[]>(`${this.baseUrl}?$top=${top}&$skip=${skip}`);
  }

}
