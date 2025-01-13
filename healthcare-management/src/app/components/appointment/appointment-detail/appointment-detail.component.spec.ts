import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentDetailComponent } from './appointment-detail.component';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import {ActivatedRoute, Router} from '@angular/router';
import { InjectionToken } from '@angular/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBar } from '@angular/material/snack-bar';
import {Appointment} from '../../../models/appointment.model';
import {of} from 'rxjs';
import {LanguageService} from '../../../services/language/language.service';
import {RoleService} from '../../../services/role/role.service';
import {AbstractControl} from '@angular/forms';

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
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);
    const roleServiceSpy = jasmine.createSpyObj('RoleService', ['isUserDoctor']);

    await TestBed.configureTestingModule({
      imports: [
        AppointmentDetailComponent,
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: AppointmentService, useValue: appointmentServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: ActivatedRoute, useValue: activatedRouteSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: RoleService, useValue: roleServiceSpy },
        provideNativeDateAdapter(),
      ],
    }).compileComponents();

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

  it('should call setLanguage on init', () => {
    const languageService = TestBed.inject(LanguageService) as jasmine.SpyObj<LanguageService>;
    expect(languageService.setLanguage).toHaveBeenCalled();
  });

  it('should fetch appointment details and update the component state', async () => {
    const mockAppointment: Appointment = {
      id: '123',
      patientId: 'mock-patient-id',
      doctorId: 'mock-doctor-id',
      date: new Date('2024-12-01'),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test notes',
    };

    appointmentService.getByIdAsync.and.returnValue(Promise.resolve(mockAppointment));
    component.fetchAppointmentDetails();

    await fixture.whenStable();

    expect(appointmentService.getByIdAsync).toHaveBeenCalledWith('123');
    expect(component.appointmentDetails).toEqual(mockAppointment);
    expect(component.loading).toBeFalse();
  });

  it('should delete the appointment and navigate to the appointments list', async () => {
    appointmentService.deleteAsync.and.returnValue(Promise.resolve());
    component.onDelete();

    await fixture.whenStable();

    expect(appointmentService.deleteAsync).toHaveBeenCalledWith('123');
    expect(router.navigate).toHaveBeenCalledWith(['/appointments']);
  });

  it('should navigate to the update page on update action', () => {
    component.onUpdate();
    expect(router.navigate).toHaveBeenCalledWith([`/appointments/update/${component.appointmentId}`]);
  });

  it('should navigate to appointments list on click', () => {
    component.onAppointmentsClick();
    expect(router.navigate).toHaveBeenCalledWith(['/appointments']);
  });

  it('should validate GUIDs correctly', () => {
    const control = { value: '123e4567-e89b-12d3-a456-426614174000' } as AbstractControl;
    const result = component.isValidGuid(control);

    expect(result).toBeNull();
  });

  it('should invalidate incorrect GUIDs', () => {
    const control = { value: 'invalid-guid' } as AbstractControl;
    const result = component.isValidGuid(control);

    expect(result).toEqual({ invalidGuid: true });
  });

  it('should handle undefined GUIDs gracefully', () => {
    const control = { value: undefined } as AbstractControl;
    const result = component.isValidGuid(control);

    expect(result).toEqual({ invalidGuid: true });
  });

  it('should handle empty GUIDs gracefully', () => {
    const control = { value: '' } as AbstractControl;
    const result = component.isValidGuid(control);

    expect(result).toEqual({ invalidGuid: true });
  });
});
