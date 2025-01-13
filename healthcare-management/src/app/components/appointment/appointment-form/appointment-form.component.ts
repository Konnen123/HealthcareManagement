import {Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from "@angular/material/datepicker";
import {MatError, MatFormField, MatHint, MatLabel, MatSuffix} from "@angular/material/form-field";
import {MatIcon} from "@angular/material/icon";
import {MatInput} from "@angular/material/input";
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CustomValidators} from '../../../shared/custom-validators/custom-validators';
import {MatOption, MatSelect, MatSelectChange} from '@angular/material/select';
import {DoctorDto} from '../../../shared/dtos/doctor.dto';
import {UserService} from '../../../services/user/user.service';
import {TranslatePipe} from '@ngx-translate/core';


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
    NgForOf,
    TranslatePipe
  ],
  templateUrl: './appointment-form.component.html',
  standalone: true,
  styleUrl: './appointment-form.component.scss'
})
export class AppointmentFormComponent implements OnInit, OnChanges{
  @Input() initialData: any = {};
  @Output() formSubmit = new EventEmitter<any>();
  @Input() mode: string = 'Create';
  doctors!: DoctorDto[];
  selectedDoctor!: DoctorDto;

  appointmentForm!: FormGroup;

  constructor(private readonly fb: FormBuilder,
              private readonly userService: UserService) {}

  async ngOnInit(): Promise<void>
  {
    this.appointmentForm = this.fb.group({
      doctorId: ['', [Validators.required, CustomValidators.isValidGuid, CustomValidators.isNotEmptyGuid]],
      date: ['', [Validators.required, CustomValidators.isValidDate, CustomValidators.isNotPastDate]],
      startTime: ['', Validators.required],
      endTime: ['', [Validators.required, CustomValidators.endTimeAfterStartTime('startTime')]],
      userNotes: ['', Validators.maxLength(500)]
    });

    this.doctors = await this.userService.getAllDoctorsAsync();
  }

  ngOnChanges(): void
  {
    if (this.initialData && this.appointmentForm)
    {
      this.appointmentForm.patchValue({
        doctorId: this.initialData.doctor.userId,
        ...this.initialData
      });

      this.selectedDoctor = this.initialData.doctor;
    }
  }

  onSubmit(): void {
    if (this.appointmentForm.valid) {
      const rawData = this.appointmentForm.value;

      const appointmentData = {
        ...rawData,
        date: this.formatDate(rawData.date),
        startTime: this.formatTime(rawData.startTime),
        endTime: this.formatTime(rawData.endTime),
      };
      this.formSubmit.emit(appointmentData);
    }
  }

    formatDate(date: Date | string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

    formatTime(time: string): string {
    const [hours, minutes] = time.split(':');
    return `${hours}:${minutes}`;
  }

  onDoctorSelectionChange($event: MatSelectChange)
  {
    if(!$event.value?.userId)
      return;

    this.appointmentForm.controls['doctorId'].setValue($event.value.userId);
  }
}
