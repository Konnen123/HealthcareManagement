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
  MatDatepickerModule,
  MatDatepickerToggle
} from '@angular/material/datepicker';
import {NgIf} from '@angular/common';

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
    private router: Router
  ) {
    this.appointmentForm = this.fb.group({
      patientId: [
        '',
        [Validators.required,this.isValidGuid, this.isNotEmptyGuid]
      ],
      doctorId: [
        '',
        [Validators.required, this.isValidGuid, this.isNotEmptyGuid]
      ],
      date: [
        '',
        [Validators.required, this.isValidDate, this.isNotPastDate]],
      startTime: [
        '',
        Validators.required],
      endTime: [
        '',
        [Validators.required, this.isEndTimeAfterStartTime.bind(this)]],
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
      console.log(appointmentData);
      this.appointmentService.createAsync(appointmentData).then((res) => {
        console.log('Server response:', res);
        //this.router.navigate(['/appointments']);
      });
    }
  }

  isValidGuid(control: AbstractControl): ValidationErrors | null {
    const guidRegex =
      /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(control.value) ? null : { invalidGuid: true };
  }

  isNotEmptyGuid(control: AbstractControl): ValidationErrors | null {
    return control.value !== '00000000-0000-0000-0000-000000000000'
      ? null
      : { emptyGuid: true };
  }

  isEndTimeAfterStartTime(control: AbstractControl): ValidationErrors | null {
    const startTime = this.appointmentForm?.get('startTime')?.value;
    const endTime = control.value;
    if (!startTime || !endTime) {
      return null;
    }
    return endTime > startTime ? null : { endTimeBeforeStartTime: true };
  }

  isValidDate(control: AbstractControl): ValidationErrors | null {
    const date = new Date(control.value);
    return !isNaN(date.getTime()) ? null : { invalidDate: true };
  }

  isNotPastDate(control: AbstractControl): ValidationErrors | null {
   // console.log('Control value:', new Date(control.value));
    const date = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const isValid = date.getTime() >= today.getTime();
    //console.log('Is valid:', isValid); // Check comparison result
    return isValid ? null : { pastDate: true };
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
