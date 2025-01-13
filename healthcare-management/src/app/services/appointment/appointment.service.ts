import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {AppointmentClient} from '../../clients/appointment/appointment.client';
import {isPlatformBrowser} from '@angular/common';
import {Appointment} from '../../models/appointment.model';
import {firstValueFrom} from 'rxjs';
import {AppointmentParams} from '../../models/appointmentParams.model';

@Injectable({
  providedIn: 'root'
})

export class AppointmentService{
  private readonly isBrowser!: boolean;
  constructor(private appointmentClient : AppointmentClient, @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId)
  }

  public async getAllAsync(startTime: string, date:string): Promise<Appointment[]>
  {
    if(!this.isBrowser)
      return new Promise<Appointment[]>((resolve, reject) => {});

    return await firstValueFrom(this.appointmentClient.getAllAppointments(startTime, date));
  }

  public async createAsync(appointment: Appointment): Promise<any> {
    if (!this.isBrowser) {
      return Promise.reject('Not running in a browser environment.');
    }
    return firstValueFrom(this.appointmentClient.createAppointment(appointment));
  }

  public async getByIdAsync(id: string): Promise<Appointment> {
    return await firstValueFrom(this.appointmentClient.getAppointmentById(id));
  }

  public async updateAsync(appointment: Appointment): Promise<any> {
    return firstValueFrom(this.appointmentClient.updateAppointment(appointment));
  }

  public async deleteAsync(id: string): Promise<any> {
    return firstValueFrom(this.appointmentClient.deleteAppointment(id));
  }

  public async getAppointmentsPaginatedAsync(appointmentParams: AppointmentParams): Promise<Appointment[]> {
    return await firstValueFrom(this.appointmentClient.getAppointmentsPaginated(appointmentParams));
  }
}
