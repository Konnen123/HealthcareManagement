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
      const result = this.appointmentClient.createAppointment(appointment);
      console.log('Server response:', result);
      return result;
    } catch (error) {
      console.error('Error while creating appointment:', error);
      throw error;
    }
  }

}
