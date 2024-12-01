import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentCreateComponent } from './appointment-create.component';
import {AppointmentService} from '../../services/appointment/appointment.service';
import {Router} from '@angular/router';
import {InjectionToken} from '@angular/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSnackBar} from '@angular/material/snack-bar';

const APP_CONFIG = new InjectionToken<any>('app.config');

describe('AppointmentCreateComponent', () => {
  let component: AppointmentCreateComponent;
  let fixture: ComponentFixture<AppointmentCreateComponent>;
  let appointmentService: jasmine.SpyObj<AppointmentService>;

  beforeEach(async () => {
    const appointmentServiceSpy = jasmine.createSpyObj('AppointmentService', ['createAsync']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        AppointmentCreateComponent,
        BrowserAnimationsModule
      ],
      providers: [
        {provide: AppointmentService, useValue: appointmentServiceSpy},
        {provide: Router, useValue: routerSpy},
        {provide: APP_CONFIG, useValue: {}},
        { provide: MatSnackBar, useValue: snackBarSpy },
        provideNativeDateAdapter()
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AppointmentCreateComponent);
    component = fixture.componentInstance;
    appointmentService = TestBed.inject(AppointmentService) as jasmine.SpyObj<AppointmentService>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have an invalid form when empty', () => {
    expect(component.appointmentForm.valid).toBeFalsy();
  });

  it('should validate patientId field', () => {
    const patientId = component.appointmentForm.controls['patientId'];
    patientId.setValue('123e4567-e89b-12d3-a456-426614174000');
    expect(patientId.valid).toBeTruthy();
    expect(patientId.hasError('isValidGuid')).toBeFalsy();
  });


  it('should submit valid form', () => {
    component.appointmentForm.setValue({
      patientId: '123e4567-e89b-12d3-a456-426614174000',
      doctorId: '123e4567-e89b-12d3-a456-426614174000',
      date: '2024-12-01',
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test notes'
    });
    appointmentService.createAsync.and.returnValue(Promise.resolve({}));

    component.onSubmit();

    expect(appointmentService.createAsync).toHaveBeenCalled();
  });

});
