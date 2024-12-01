import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {AppointmentService} from '../../services/appointment/appointment.service';
import {Router} from '@angular/router';
import {MatError, MatFormField, MatHint, MatLabel, MatSuffix} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatIcon, MatIconModule} from '@angular/material/icon';
import {MatButton} from '@angular/material/button';
import {
  MatDatepicker,
  MatDatepickerInput,
  MatDatepickerToggle
} from '@angular/material/datepicker';
import {NgIf} from '@angular/common';
import {CustomValidators} from '../../shared/custom-validators';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-appointment-create',
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatIconModule,
    MatLabel,
    MatError,
    MatIcon,
    MatButton,
    MatHint,
    MatDatepickerToggle,
    MatDatepicker,
    MatSuffix,
    MatDatepickerInput,
    NgIf
  ],
  templateUrl: './appointment-create.component.html',
  styleUrl: './appointment-create.component.scss'
})
export class AppointmentCreateComponent {
  appointmentForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    this.appointmentForm = this.fb.group({
      patientId: [
        '',
        [Validators.required,CustomValidators.isValidGuid, CustomValidators.isNotEmptyGuid]
      ],
      doctorId: [
        '',
        [Validators.required, CustomValidators.isValidGuid, CustomValidators.isNotEmptyGuid]
      ],
      date: [
        '',
        [Validators.required, CustomValidators.isValidDate, CustomValidators.isNotPastDate]],
      startTime: [
        '',
        Validators.required],
      endTime: [
        '',
        [Validators.required, CustomValidators.endTimeAfterStartTime('startTime')]],
      userNotes: ['', Validators.maxLength(500)]
    });
  }

  ngOnit(): void {}

  onSubmit() {
    if (this.appointmentForm.valid) {
      //console.log(this.appointmentForm.value);
      const rawData = this.appointmentForm.value;

      const appointmentData = {
        ...rawData,
        date: this.formatDate(rawData.date), // Format date
        startTime: this.formatTime(rawData.startTime), // Format start time
        endTime: this.formatTime(rawData.endTime), // Format end time
      };
      //console.log(appointmentData);
      this.appointmentService.createAsync(appointmentData).then((res) => {
        console.log('Server response:', res);
        //this.router.navigate(['/appointments']);
      }).catch((error) => {
        console.error('Error while creating appointment in component:', error);

        if (error.status === 400) {
          this.snackBar.open('Invalid appointment data. Please check the form and try again.', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar'],
          });
        } else {
          this.snackBar.open('An unexpected error occurred. Please try again later.', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar'],
          });
        }
      })
    }
  }



  formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0'); // Month is 0-based
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  formatTime(time: string): string {
    const [hours, minutes] = time.split(':'); // Assume the input is `HH:mm`
    return `${hours}:${minutes}`;
  }
}
