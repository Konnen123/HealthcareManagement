import { TestBed } from '@angular/core/testing';
import { AppointmentClient } from './appointment.client';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AppConfig } from '../../app-config/app.config.interface';
import { APP_SERVICE_CONFIG } from '../../app-config/app.config';
import { Appointment } from '../../models/appointment.model';
import { AppointmentParams } from '../../models/appointmentParams.model';

describe('AppointmentClient', () => {
  let client: AppointmentClient;
  let httpMock: HttpTestingController;
  const mockConfig: AppConfig = { apiEndpoint: 'http://localhost/api' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AppointmentClient,
        { provide: APP_SERVICE_CONFIG, useValue: mockConfig },
      ],
    });

    client = TestBed.inject(AppointmentClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the service', () => {
    expect(client).toBeTruthy();
  });

  it('should fetch all appointments with filters', () => {
    const mockResponse: Appointment[] = [
      { id: '1', patientId: '123', doctorId: '456', date: new Date(), startTime: '10:00', endTime: '11:00' },
    ];

    client.getAllAppointments('10:00', '2024-01-01').subscribe((appointments) => {
      expect(appointments).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Appointments?$filter=startTime eq 10:00 and date eq 2024-01-01`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should create an appointment', () => {
    const newAppointment: Appointment = {
      id: '1',
      patientId: '123',
      doctorId: '456',
      date: new Date('2024-01-01'),
      startTime: '10:00',
      endTime: '11:00',
    };

    client.createAppointment(newAppointment).subscribe((response) => {
      expect(response).toEqual(newAppointment);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Appointments`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newAppointment);
    req.flush(newAppointment);
  });

  it('should fetch an appointment by ID', () => {
    const mockAppointment: Appointment = {
      id: '1',
      patientId: '123',
      doctorId: '456',
      date: new Date('2024-01-01'),
      startTime: '10:00',
      endTime: '11:00',
    };

    client.getAppointmentById('1').subscribe((appointment) => {
      expect(appointment).toEqual(mockAppointment);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Appointments/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockAppointment);
  });

  it('should update an appointment', () => {
    const updatedAppointment: Appointment = {
      id: '1',
      patientId: '123',
      doctorId: '456',
      date: new Date('2024-01-01'),
      startTime: '10:00',
      endTime: '11:00',
    };

    client.updateAppointment(updatedAppointment).subscribe((response) => {
      expect(response).toEqual(updatedAppointment);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Appointments/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedAppointment);
    req.flush(updatedAppointment);
  });

  it('should delete an appointment', () => {
    client.deleteAppointment('1').subscribe((response) => {
      expect(response).toBeNull();
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Appointments/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });

  it('should fetch paginated appointments with filters', () => {
    const mockResponse: Appointment[] = [
      { id: '1', patientId: '123', doctorId: '456', date: new Date(), startTime: '10:00', endTime: '11:00' },
    ];
    const mockParams: AppointmentParams = { top: 10, skip: 0, startTime: '10:00', date: '2024-01-01' };

    client.getAppointmentsPaginated(mockParams).subscribe((appointments) => {
      expect(appointments).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Appointments?$filter=startTime eq 10:00 and date eq 2024-01-01&$top=10&$skip=0`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
});
