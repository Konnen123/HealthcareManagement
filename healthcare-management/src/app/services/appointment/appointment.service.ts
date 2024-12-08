import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {AppointmentClient} from '../../clients/appointment.client';
import {isPlatformBrowser} from '@angular/common';
import {Appointment} from '../../models/appointment.model';
import {firstValueFrom} from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AppointmentService{
  private readonly isBrowser!: boolean;
  constructor(private appointmentClient : AppointmentClient, @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId)
  }

  public async getAllAsync(): Promise<Appointment[]>
  {
    if(!this.isBrowser)
      return new Promise<Appointment[]>((resolve, reject) => {});

    return await firstValueFrom(this.appointmentClient.getAllAppointments());
  }

  public async createAsync(appointment: Appointment): Promise<any> {
    if (!this.isBrowser) {
      return Promise.reject('Not running in a browser environment.');
    }
    try {
      const result = firstValueFrom(this.appointmentClient.createAppointment(appointment));
      console.log('Server response in the service :', result);
      return result;
    } catch (error) {
      console.error('Error while creating appointment in service', error);
      throw error;
    }
  }

  public async getByIdAsync(id: string): Promise<Appointment> {
    try {
      return await firstValueFrom(this.appointmentClient.getAppointmentById(id));
    } catch (error) {
      console.error('Error while getting appointment in service:', error);
      throw error;
    }
  }

  public async updateAsync(appointment: Appointment): Promise<any> {
    try {
      const result = firstValueFrom(this.appointmentClient.updateAppointment(appointment));
      console.log('Server response:', result);
      return result;
    } catch (error) {
      console.error('Error while updating appointment in service:', error);
      throw error;
    }
  }

  public async deleteAsync(id: string): Promise<any> {
    try {
      const result = firstValueFrom(this.appointmentClient.deleteAppointment(id));
      console.log('Server response:', result);
      return result;
    } catch (error) {
      console.error('Error while deleting appointment in service:', error);
      throw error;
    }
  }

  public async getAppointmentsPaginatedAsync(pageSize: number, pageIndex: number): Promise<Appointment[]> {
    try {
      return await firstValueFrom(this.appointmentClient.getAppointmentsPaginated(pageSize, pageIndex));
    } catch (error) {
      console.error('Error while getting appointments paginated in service:', error);
      throw error;
    }
  }
}
