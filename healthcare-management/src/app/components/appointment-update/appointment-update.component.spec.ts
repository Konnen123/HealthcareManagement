import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentUpdateComponent } from './appointment-update.component';
import { AppointmentService } from '../../services/appointment/appointment.service';
import { ActivatedRoute, Router } from '@angular/router';
import { InjectionToken } from '@angular/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import {Appointment} from '../../models/appointment.model';

const APP_CONFIG = new InjectionToken<any>('app.config');

describe('AppointmentUpdateComponent', () => {
  let component: AppointmentUpdateComponent;
  let fixture: ComponentFixture<AppointmentUpdateComponent>;
  let appointmentService: jasmine.SpyObj<AppointmentService>;
  let router: jasmine.SpyObj<Router>;
  let snackBar: jasmine.SpyObj<MatSnackBar>;
  let activatedRoute: ActivatedRoute;

  beforeEach(async () => {
    const appointmentServiceSpy = jasmine.createSpyObj('AppointmentService', ['getByIdAsync', 'updateAsync']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
    const activatedRouteStub = {
      params: of({ id: '123' }) // Mock the params observable
    };

    await TestBed.configureTestingModule({
      imports: [
        AppointmentUpdateComponent,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AppointmentService, useValue: appointmentServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: APP_CONFIG, useValue: {} },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: ActivatedRoute, useValue: activatedRouteStub },
        provideNativeDateAdapter()
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AppointmentUpdateComponent);
    component = fixture.componentInstance;
    appointmentService = TestBed.inject(AppointmentService) as jasmine.SpyObj<AppointmentService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    snackBar = TestBed.inject(MatSnackBar) as jasmine.SpyObj<MatSnackBar>;
    activatedRoute = TestBed.inject(ActivatedRoute);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load appointment details on initialization', async () => {
    const mockAppointment : Appointment = {
      patientId: '123',
      doctorId: '456',
      date: new Date('2024-12-01'),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Sample notes'
    };

    appointmentService.getByIdAsync.and.returnValue(Promise.resolve(mockAppointment));

    await component.ngOnInit();
    fixture.detectChanges();

    expect(appointmentService.getByIdAsync).toHaveBeenCalledWith('123');
    expect(component.appointmentForm.value).toEqual(mockAppointment);
    expect(component.loading).toBeFalse();
  });

  it('should submit the form and call update service', async () => {
    const mockAppointment = {
      patientId: '123e4567-e89b-12d3-a456-426614174000',
      doctorId: '123e4567-e89b-12d3-a456-426614174000',
      date: new Date('2024-12-01'),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Sample notes'
    };
    appointmentService.updateAsync.and.returnValue(Promise.resolve({ success: true }));
    component.appointmentForm.setValue(mockAppointment);

    component.onSubmit();

    expect(appointmentService.updateAsync).toHaveBeenCalledWith({
      ...mockAppointment,
      date: new Date('2024-12-01'),
      startTime: '10:00',
      endTime: '11:00',
      id: component.appointmentId
    });
  });
});
