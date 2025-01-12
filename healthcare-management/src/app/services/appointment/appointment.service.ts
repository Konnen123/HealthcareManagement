import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {AppointmentClient} from '../../clients/appointment.client';
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
    try {
      return firstValueFrom(this.appointmentClient.createAppointment(appointment));
    } catch (error) {
      throw error;
    }
  }

  public async getByIdAsync(id: string): Promise<Appointment> {
    try {
      return await firstValueFrom(this.appointmentClient.getAppointmentById(id));
    } catch (error) {
      throw error;
    }
  }

  public async updateAsync(appointment: Appointment): Promise<any> {
    try {
      return firstValueFrom(this.appointmentClient.updateAppointment(appointment));
    } catch (error) {
      throw error;
    }
  }

  public async deleteAsync(id: string): Promise<any> {
    try {
      return firstValueFrom(this.appointmentClient.deleteAppointment(id));
    } catch (error) {
      throw error;
    }
  }

  public async getAppointmentsPaginatedAsync(appointmentParams: AppointmentParams): Promise<Appointment[]> {
    try {
      return await firstValueFrom(this.appointmentClient.getAppointmentsPaginated(appointmentParams));
    } catch (error) {
      throw error;
    }
  }
}
