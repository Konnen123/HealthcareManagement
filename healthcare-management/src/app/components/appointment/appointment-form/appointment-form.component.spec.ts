import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { AppointmentFormComponent } from './appointment-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatNativeDateModule, provideNativeDateAdapter} from '@angular/material/core';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

describe('AppointmentFormComponent', () => {
  let component: AppointmentFormComponent;
  let fixture: ComponentFixture<AppointmentFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AppointmentFormComponent,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        BrowserAnimationsModule
      ],
      providers: [
        provideNativeDateAdapter()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty fields', () => {
    expect(component.appointmentForm.value).toEqual({
      patientId: '',
      doctorId: '',
      date: '',
      startTime: '',
      endTime: '',
      userNotes: '',
    });
  });

  it('should emit formSubmit when form is valid and submitted', () => {
    spyOn(component.formSubmit, 'emit');

    component.appointmentForm.patchValue({
      patientId: 'd99ccb79-67e3-4d7c-8725-14f01981448f',
      doctorId: 'ac5fe411-9b82-40de-8963-27b9a3074bae',
      date: '2024-12-31',
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test note',
    });

    component.onSubmit();
    expect(component.formSubmit.emit).toHaveBeenCalledWith({
      patientId: 'd99ccb79-67e3-4d7c-8725-14f01981448f',
      doctorId: 'ac5fe411-9b82-40de-8963-27b9a3074bae',
      date: '2024-12-31',
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test note',
    });
  });

  it('should not emit formSubmit when form is invalid', () => {
    spyOn(component.formSubmit, 'emit');
    component.onSubmit();
    expect(component.formSubmit.emit).not.toHaveBeenCalled();
  });
});
