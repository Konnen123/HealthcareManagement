import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentDetailComponent } from './appointment-detail.component';
import { AppointmentService } from '../../services/appointment/appointment.service';
import {ActivatedRoute, Router} from '@angular/router';
import { InjectionToken } from '@angular/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBar } from '@angular/material/snack-bar';
import {Appointment} from '../../models/appointment.model';
import {of} from 'rxjs';

const APP_CONFIG = new InjectionToken<any>('app.config');

describe('AppointmentDetailComponent', () => {
  let component: AppointmentDetailComponent;
  let fixture: ComponentFixture<AppointmentDetailComponent>;
  let appointmentService: jasmine.SpyObj<AppointmentService>;
  let router: jasmine.SpyObj<Router>;
  let snackBar: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    const appointmentServiceSpy = jasmine.createSpyObj('AppointmentService', ['deleteAsync', 'getByIdAsync']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
    const activatedRouteSpy = { params: of({ id: '123' }) };
    await TestBed.configureTestingModule({
      imports: [
        AppointmentDetailComponent,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AppointmentService, useValue: appointmentServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: APP_CONFIG, useValue: {} },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: ActivatedRoute, useValue: activatedRouteSpy },
        provideNativeDateAdapter()
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AppointmentDetailComponent);
    component = fixture.componentInstance;
    appointmentService = TestBed.inject(AppointmentService) as jasmine.SpyObj<AppointmentService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    snackBar = TestBed.inject(MatSnackBar) as jasmine.SpyObj<MatSnackBar>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch appointment details and set appointmentDetails', async () => {
    const mockAppointment: Appointment = {
      id: '123',
      patientId: '123e4567-e89b-12d3-a456-426614174000',
      doctorId: '123e4567-e89b-12d3-a456-426614174000',
      date: new Date('2024-12-01'),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test notes'
    };

    appointmentService.getByIdAsync.and.returnValue(Promise.resolve(mockAppointment));
    component.fetchAppointmentDetails();

    await fixture.whenStable();

    expect(appointmentService.getByIdAsync).toHaveBeenCalledWith('123');
    expect(component.appointmentDetails).toEqual(mockAppointment);
    expect(component.loading).toBeFalse();
  });

  it('should delete appointment and navigate to appointments list', async () => {
    appointmentService.deleteAsync.and.returnValue(Promise.resolve());
    component.onDelete();
    await fixture.whenStable();

    expect(appointmentService.deleteAsync).toHaveBeenCalledWith('123');
    expect(router.navigate).toHaveBeenCalledWith(['/appointments']);
  });


});
