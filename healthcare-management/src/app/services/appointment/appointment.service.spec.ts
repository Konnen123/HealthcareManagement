import { TestBed } from '@angular/core/testing';
import { AppointmentService } from './appointment.service';
import { AppointmentClient } from '../../clients/appointment/appointment.client';
import { PLATFORM_ID } from '@angular/core';
import { of } from 'rxjs';
import { Appointment } from '../../models/appointment.model';
import { AppointmentParams } from '../../models/appointmentParams.model';

describe('AppointmentService', () => {
  let service: AppointmentService;
  let mockAppointmentClient: jasmine.SpyObj<AppointmentClient>;

  beforeEach(() => {
    mockAppointmentClient = jasmine.createSpyObj('AppointmentClient', [
      'getAllAppointments',
      'createAppointment',
      'getAppointmentById',
      'updateAppointment',
      'deleteAppointment',
      'getAppointmentsPaginated',
    ]);

    TestBed.configureTestingModule({
      providers: [
        AppointmentService,
        { provide: AppointmentClient, useValue: mockAppointmentClient },
        { provide: PLATFORM_ID, useValue: 'browser' }, // Simulate browser environment
      ],
    });

    service = TestBed.inject(AppointmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all appointments', async () => {
    const mockAppointments: Appointment[] = [
      { id: '1', doctorId: '123', patientId: '456', date: new Date(), startTime: '10:00', endTime: '11:00' },
    ];
    mockAppointmentClient.getAllAppointments.and.returnValue(of(mockAppointments));

    const result = await service.getAllAsync('10:00', '2024-01-01');

    expect(mockAppointmentClient.getAllAppointments).toHaveBeenCalledWith('10:00', '2024-01-01');
    expect(result).toEqual(mockAppointments);
  });

  it('should create an appointment', async () => {
    const mockAppointment: Appointment = {
      id: '1',
      doctorId: '123',
      patientId: '456',
      date: new Date(),
      startTime: '10:00',
      endTime: '11:00',
    };
    mockAppointmentClient.createAppointment.and.returnValue(of(mockAppointment));

    const result = await service.createAsync(mockAppointment);

    expect(mockAppointmentClient.createAppointment).toHaveBeenCalledWith(mockAppointment);
    expect(result).toEqual(mockAppointment);
  });

  it('should fetch an appointment by ID', async () => {
    const mockAppointment: Appointment = {
      id: '1',
      doctorId: '123',
      patientId: '456',
      date: new Date(),
      startTime: '10:00',
      endTime: '11:00',
    };
    mockAppointmentClient.getAppointmentById.and.returnValue(of(mockAppointment));

    const result = await service.getByIdAsync('1');

    expect(mockAppointmentClient.getAppointmentById).toHaveBeenCalledWith('1');
    expect(result).toEqual(mockAppointment);
  });

  it('should update an appointment', async () => {
    const mockAppointment: Appointment = {
      id: '1',
      doctorId: '123',
      patientId: '456',
      date: new Date(),
      startTime: '10:00',
      endTime: '11:00',
    };
    mockAppointmentClient.updateAppointment.and.returnValue(of(mockAppointment));

    const result = await service.updateAsync(mockAppointment);

    expect(mockAppointmentClient.updateAppointment).toHaveBeenCalledWith(mockAppointment);
    expect(result).toEqual(mockAppointment);
  });

  it('should delete an appointment', async () => {
    const mockResponse = { success: true };
    mockAppointmentClient.deleteAppointment.and.returnValue(of(mockResponse));

    const result = await service.deleteAsync('1');

    expect(mockAppointmentClient.deleteAppointment).toHaveBeenCalledWith('1');
    expect(result).toEqual(mockResponse);
  });

  it('should fetch paginated appointments with default AppointmentParams', async () => {
    const defaultParams = new AppointmentParams();
    const mockAppointments: Appointment[] = [
      { id: '1', doctorId: '123', patientId: '456', date: new Date(), startTime: '10:00', endTime: '11:00' },
    ];
    mockAppointmentClient.getAppointmentsPaginated.and.returnValue(of(mockAppointments));

    const result = await service.getAppointmentsPaginatedAsync(defaultParams);

    expect(mockAppointmentClient.getAppointmentsPaginated).toHaveBeenCalledWith(defaultParams);
    expect(result).toEqual(mockAppointments);
  });

  it('should fetch paginated appointments with custom AppointmentParams', async () => {
    const customParams: AppointmentParams = {
      date: '2024-01-01',
      startTime: '09:00',
      skip: 5,
      top: 20,
    };
    const mockAppointments: Appointment[] = [
      { id: '1', doctorId: '123', patientId: '456', date: new Date(), startTime: '10:00', endTime: '11:00' },
      { id: '2', doctorId: '124', patientId: '457', date: new Date(), startTime: '11:00', endTime: '12:00' },
    ];
    mockAppointmentClient.getAppointmentsPaginated.and.returnValue(of(mockAppointments));

    const result = await service.getAppointmentsPaginatedAsync(customParams);

    expect(mockAppointmentClient.getAppointmentsPaginated).toHaveBeenCalledWith(customParams);
    expect(result).toEqual(mockAppointments);
  });

  it('should return an empty array if AppointmentParams is invalid', async () => {
    const invalidParams: AppointmentParams = {
      date: '',
      startTime: '',
      skip: -1, // Invalid value
      top: -5,  // Invalid value
    };

    mockAppointmentClient.getAppointmentsPaginated.and.returnValue(of([]));

    const result = await service.getAppointmentsPaginatedAsync(invalidParams);

    expect(mockAppointmentClient.getAppointmentsPaginated).toHaveBeenCalledWith(invalidParams);
    expect(result).toEqual([]);
  });


});
