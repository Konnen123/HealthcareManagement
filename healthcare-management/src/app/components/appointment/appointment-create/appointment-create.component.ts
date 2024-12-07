import { Component } from '@angular/core';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import {AppointmentFormComponent} from '../appointment-form/appointment-form.component';

@Component({
  selector: 'app-appointment-create',
  templateUrl: './appointment-create.component.html',
  imports: [
    AppointmentFormComponent
  ]
})
export class AppointmentCreateComponent {
  constructor(
    readonly appointmentService: AppointmentService,
    readonly snackBar: MatSnackBar,
    readonly router: Router
  ) {}

  onFormSubmit(formData: any): void {
    console.log('Form data:', formData);
    this.appointmentService.createAsync(formData).then(() => {
      this.snackBar.open('Appointment created successfully.', 'Close', {
        duration: 5000,
      });
      //this.router.navigate(['/appointments']);
    }).catch(() => {
      this.snackBar.open('Failed to create appointment. Please try again.', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    });
  }
}
