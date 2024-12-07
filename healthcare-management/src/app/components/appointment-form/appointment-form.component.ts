import {Component, EventEmitter, Input, Output} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from "@angular/material/datepicker";
import {MatError, MatFormField, MatHint, MatLabel, MatSuffix} from "@angular/material/form-field";
import {MatIcon} from "@angular/material/icon";
import {MatInput} from "@angular/material/input";
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CustomValidators} from '../../shared/custom-validators';
import {formatDate, formatTime} from '../../shared/date-time.utils';
import {MatOption, MatSelect} from '@angular/material/select';

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
    ReactiveFormsModule,
    MatSelect,
    MatOption,
    NgForOf
  ],
  templateUrl: './appointment-form.component.html',
  styleUrl: './appointment-form.component.scss'
})
export class AppointmentFormComponent {
  @Input() initialData: any = {};
  @Output() formSubmit = new EventEmitter<any>();
  @Input() mode: 'create' | 'update' = 'create'; // Default to 'create'

  appointmentForm: FormGroup;
  doctors = [
    { id: '1', name: 'Dr. John Smith' },
    { id: '2', name: 'Dr. Jane Doe' },
    { id: '3', name: 'Dr. Emily Johnson' },
  ];

  constructor(readonly fb: FormBuilder) {
    this.appointmentForm = this.fb.group({
      patientId: ['', [Validators.required, CustomValidators.isValidGuid, CustomValidators.isNotEmptyGuid]],
      doctorId: ['', [Validators.required]],
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
        date: formatDate(rawData.date),
        startTime: formatTime(rawData.startTime),
        endTime: formatTime(rawData.endTime),
      };
      //console.log(appointmentData);
      this.formSubmit.emit(appointmentData);
    }
  }

}
