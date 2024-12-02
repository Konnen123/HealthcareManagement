import {Component, EventEmitter, Input, Output} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from "@angular/material/datepicker";
import {MatError, MatFormField, MatHint, MatLabel, MatSuffix} from "@angular/material/form-field";
import {MatIcon} from "@angular/material/icon";
import {MatInput} from "@angular/material/input";
import {NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CustomValidators} from '../../shared/custom-validators';

@Component({
  selector: 'app-appointment-form',
    imports: [
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
  templateUrl: './appointment-form.component.html',
  styleUrl: './appointment-form.component.scss'
})
export class AppointmentFormComponent {
  @Input() initialData: any = {};
  @Output() formSubmit = new EventEmitter<any>();

  appointmentForm: FormGroup;

  constructor(readonly fb: FormBuilder) {
    this.appointmentForm = this.fb.group({
      patientId: ['', [Validators.required, CustomValidators.isValidGuid, CustomValidators.isNotEmptyGuid]],
      doctorId: ['', [Validators.required, CustomValidators.isValidGuid, CustomValidators.isNotEmptyGuid]],
      date: ['', [Validators.required, CustomValidators.isValidDate, CustomValidators.isNotPastDate]],
      startTime: ['', Validators.required],
      endTime: ['', [Validators.required, CustomValidators.endTimeAfterStartTime('startTime')]],
      userNotes: ['', Validators.maxLength(500)]
    });
  }

  ngOnChanges(): void {
    if (this.initialData) {
      this.appointmentForm.patchValue(this.initialData);
    }
  }

  onSubmit(): void {
    if (this.appointmentForm.valid) {

      console.log(this.appointmentForm.value);
      const rawData = this.appointmentForm.value;

      const appointmentData = {
        ...rawData,
        date: this.formatDate(rawData.date),
        startTime: this.formatTime(rawData.startTime),
        endTime: this.formatTime(rawData.endTime),
      };
      //console.log(appointmentData);
      this.formSubmit.emit(appointmentData);
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
