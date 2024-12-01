import {Component, OnInit} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {AppointmentService} from '../../services/appointment/appointment.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {HttpErrorResponse} from '@angular/common/http';
import {MatButton} from '@angular/material/button';
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from '@angular/material/datepicker';
import {MatError, MatFormField, MatHint, MatLabel, MatSuffix} from '@angular/material/form-field';
import {MatIcon} from '@angular/material/icon';
import {MatInput} from '@angular/material/input';
import {NgIf} from '@angular/common';
import {CustomValidators} from '../../shared/custom-validators';

@Component({
  selector: 'app-appointment-update',
  imports: [
    FormsModule,
    MatButton,
    MatDatepicker,
    MatDatepickerInput,
    MatDatepickerToggle,
    MatError,
    MatFormField,
    MatHint,
    MatIcon,
    MatInput,
    MatLabel,
    MatSuffix,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './appointment-update.component.html',
  styleUrl: './appointment-update.component.scss'
})
export class AppointmentUpdateComponent implements OnInit {
  appointmentForm!: FormGroup;
  appointmentId!: string;
  loading: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    // Get the appointment from the URL
    this.route.params.subscribe(params => {
      this.appointmentId = params['id'];
      this.loadAppointmentDetails();
    });

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

  private loadAppointmentDetails() {
    //Fetch appointment details
    this.appointmentService.getByIdAsync(this.appointmentId).then(appointment => {
      this.appointmentForm.patchValue(appointment);
      this.loading = false;
    }).catch((error: HttpErrorResponse) => {
      this.loading = false;
      this.showErrorSnackbar(error.status === 404
        ? `Appointment not found.`
        : 'An unexpected error occurred.');
    });
  }

  private showErrorSnackbar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['error-snackbar'],
    });
  }

  onSubmit(){
    if (this.appointmentForm.valid) {
      //console.log(this.appointmentForm.value);
      const rawData = this.appointmentForm.value;

      const appointmentData = {
        ...rawData,
        date: this.formatDate(rawData.date), // Format date
        startTime: this.formatTime(rawData.startTime), // Format start time
        endTime: this.formatTime(rawData.endTime), // Format end time
        id: this.appointmentId
      };
      //console.log(appointmentData);
      this.appointmentService.updateAsync(appointmentData).then((res) => {
        console.log('Server response:', res);
        //this.router.navigate(['/appointments']);
      }).catch((error) => {
        console.error('Error while creating appointment in component:', error);
        if (error.status === 400){
          this.snackBar.open('Unable to update the appointment. Please check the form and try again.', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar'],
          });
        } else {
          this.snackBar.open('An unexpected error occurred. Please try again later.', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar'],
          });
        }
      });
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
