import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AppConfig} from '../../app-config/app.config.interface';
import {APP_SERVICE_CONFIG} from '../../app-config/app.config';
import {Observable} from 'rxjs';
import {Appointment} from '../../models/appointment.model';
import {AppointmentParams} from '../../models/appointmentParams.model';

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

  public getAllAppointments(startTime:string, date:string): Observable<Appointment[]>
  {
    const filters = this.constructFilters(startTime, date);
    return this.http.get<Appointment[]>(`${this.baseUrl}?${filters}`);
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

  public getAppointmentsPaginated(appointmentParams: AppointmentParams): Observable<Appointment[]>{
    const filters = this.constructFilters(appointmentParams.startTime, appointmentParams.date);
    const pagination = this.constructPagination(appointmentParams);

    const queryParams = [...filters, ...pagination].join('&');

    return this.http.get<Appointment[]>(`${this.baseUrl}?${queryParams}`);
  }

  private constructFilters(startTime:string, date: string): string[] {
    const filters: string[] = [];

    if (startTime) {
      filters.push(`$filter=startTime eq ${startTime}`);
    }

    if (date) {
      const dateFilter = `date eq ${date}`;
      if (startTime) {
        filters[0] = `${filters[0]} and ${dateFilter}`;
      } else {
        filters.push(`$filter=${dateFilter}`);
      }
    }

    return filters;
  }
  private constructPagination(params: AppointmentParams): string[] {
    return [`$top=${params.top}`, `$skip=${params.skip}`];
  }

}
