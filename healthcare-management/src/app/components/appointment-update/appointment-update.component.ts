import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppointmentService } from '../../services/appointment/appointment.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AppointmentFormComponent } from '../appointment-form/appointment-form.component';

@Component({
  selector: 'app-appointment-update',
  templateUrl: './appointment-update.component.html',
  imports: [
    AppointmentFormComponent
  ]
})
export class AppointmentUpdateComponent implements OnInit {
  appointmentData: any;

  constructor(
    readonly route: ActivatedRoute,
    readonly appointmentService: AppointmentService,
    readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const appointmentId = this.route.snapshot.paramMap.get('id');
    if (appointmentId) {
      //console.log()
      this.appointmentService.getByIdAsync(appointmentId).then((data) => {
        this.appointmentData = data;
      }).catch(() => {
        this.snackBar.open('Failed to load appointment details.', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar'],
        });
      });
    }
  }

  onFormSubmit(formData: any): void {
    this.appointmentService.updateAsync({ ...formData, id: this.appointmentData.id }).then(() => {
      this.snackBar.open('Appointment updated successfully.', 'Close', {
        duration: 5000,
      });
    }).catch(() => {
      this.snackBar.open('Failed to update appointment. Please try again.', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    });
  }
}
