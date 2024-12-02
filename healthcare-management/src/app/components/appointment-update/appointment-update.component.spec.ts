import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentUpdateComponent } from './appointment-update.component';
import { AppointmentService } from '../../services/appointment/appointment.service';
import { ActivatedRoute } from '@angular/router';
import {MatSnackBar, MatSnackBarModule} from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';
import {AppointmentFormComponent} from '../appointment-form/appointment-form.component';
import {MatInputModule} from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

describe('AppointmentUpdateComponent', () => {
  let component: AppointmentUpdateComponent;
  let fixture: ComponentFixture<AppointmentUpdateComponent>;
  let mockAppointmentService: jasmine.SpyObj<AppointmentService>;
  let mockActivatedRoute: any;
  let snackBarSpy: any;

  beforeEach(async () => {
    // Mock the AppointmentService
    mockAppointmentService = jasmine.createSpyObj('AppointmentService', ['getByIdAsync', 'updateAsync']);

    // Mock ActivatedRoute with a snapshot paramMap
    mockActivatedRoute = {
      snapshot: { paramMap: { get: jasmine.createSpy('get').and.returnValue('4cc94e19-1a61-427e-8b52-4aac545dbf51') } },
    };

    // Mock MatSnackBar
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        AppointmentUpdateComponent,
        AppointmentFormComponent,
        MatSnackBarModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AppointmentService, useValue: mockAppointmentService },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: MatSnackBar, useValue: snackBarSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load appointment details on initialization', async () => {
    const mockAppointmentData = {
      id: '4cc94e19-1a61-427e-8b52-4aac545dbf51',
      patientId: 'd99ccb79-67e3-4d7c-8725-14f01981448f',
      doctorId: 'ac5fe411-9b82-40de-8963-27b9a3074bae',
      date: new Date('2024-12-31'),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test note',
    };
    mockAppointmentService.getByIdAsync.and.returnValue(Promise.resolve(mockAppointmentData));

    await component.ngOnInit();

    expect(mockAppointmentService.getByIdAsync).toHaveBeenCalledWith('4cc94e19-1a61-427e-8b52-4aac545dbf51');
    expect(component.appointmentData).toEqual(mockAppointmentData);
  });

  // it('should show error snackbar if fetching appointment fails', async () => {
  //   mockAppointmentService.getByIdAsync.and.returnValue(Promise.reject(new Error('Test error')));
  //
  //   component.ngOnInit();
  //
  //   expect(snackBarSpy.open).toHaveBeenCalledWith(
  //     'Failed to load appointment details.',
  //     'Close',
  //     { duration: 5000, panelClass: ['error-snackbar'] }
  //   );
  // });
  //
  // it('should call updateAsync and show success snackbar on valid form submission', async () => {
  //   const mockFormData = {
  //     patientId: '123',
  //     doctorId: '456',
  //     date: new Date('2024-12-31'),
  //     startTime: '10:00',
  //     endTime: '11:00',
  //     userNotes: 'Updated note',
  //   };
  //   const mockAppointmentData = { id: '1' };
  //   component.appointmentData = mockAppointmentData;
  //
  //   mockAppointmentService.updateAsync.and.returnValue(Promise.resolve({}));
  //
  //   await component.onFormSubmit(mockFormData);
  //
  //   expect(mockAppointmentService.updateAsync).toHaveBeenCalledWith({
  //     ...mockFormData,
  //     id: '1',
  //   });
  //   expect(snackBarSpy.open).toHaveBeenCalledWith(
  //     'Appointment updated successfully.',
  //     'Close',
  //     { duration: 5000 }
  //   );
  // });
  //
  // it('should show error snackbar if updateAsync fails', async () => {
  //   const mockFormData = {
  //     patientId: '123',
  //     doctorId: '456',
  //     date: new Date('2024-12-31'),
  //     startTime: '10:00',
  //     endTime: '11:00',
  //     userNotes: 'Updated note',
  //   };
  //   const mockAppointmentData = { id: '1' };
  //   component.appointmentData = mockAppointmentData;
  //
  //   mockAppointmentService.updateAsync.and.returnValue(Promise.reject(new Error('Test error')));
  //
  //   await component.onFormSubmit(mockFormData);
  //
  //   expect(snackBarSpy.open).toHaveBeenCalledWith(
  //     'Failed to update appointment. Please try again.',
  //     'Close',
  //     { duration: 5000, panelClass: ['error-snackbar'] }
  //   );
  // });
});
