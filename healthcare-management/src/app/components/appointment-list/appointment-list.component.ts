import {Component, OnInit} from '@angular/core';
import {AppointmentService} from '../../services/appointment/appointment.service';
import {Appointment} from '../../models/appointment.model';

@Component({
    selector: 'app-appointment-list',
    imports: [],
    templateUrl: './appointment-list.component.html',
    styleUrl: './appointment-list.component.scss'
})
export class AppointmentListComponent implements OnInit
{
  appointments!: Appointment[];
  constructor(private appointmentService: AppointmentService) {}

  async ngOnInit(): Promise<void>
  {
    this.appointments = await this.appointmentService.getAllAsync();
  }

}
