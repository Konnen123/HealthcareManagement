import { TestBed } from '@angular/core/testing';
import { UserClient } from './user.client';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { APP_SERVICE_CONFIG } from '../../app-config/app.config';
import { AppConfig } from '../../app-config/app.config.interface';
import { DoctorDto } from '../../shared/dtos/doctor.dto';

describe('UserClient', () => {
  let client: UserClient;
  let httpMock: HttpTestingController;
  const mockConfig: AppConfig = { apiEndpoint: 'http://localhost/api' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        UserClient,
        { provide: APP_SERVICE_CONFIG, useValue: mockConfig },
      ],
    });

    client = TestBed.inject(UserClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(client).toBeTruthy();
  });

  it('should fetch all doctors', () => {
    const mockDoctors: DoctorDto[] = [
      {
        id: '1',
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        phoneNumber: '1234567890',
        dateOfBirth: new Date('1980-01-01'),
        createdAt: new Date('2020-01-01'),
        role: 'doctor',
      },
    ];

    client.getAllDoctors().subscribe((doctors) => {
      expect(doctors).toEqual(mockDoctors);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Users/Doctors`);
    expect(req.request.method).toBe('GET');
    req.flush(mockDoctors);
  });

  it('should handle an empty doctors list', () => {
    client.getAllDoctors().subscribe((doctors) => {
      expect(doctors).toEqual([]);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Users/Doctors`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should handle an error when fetching doctors', () => {
    client.getAllDoctors().subscribe({
      next: () => fail('Should have failed with a 500 error'),
      error: (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      },
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Users/Doctors`);
    expect(req.request.method).toBe('GET');
    req.flush('Server error', { status: 500, statusText: 'Internal Server Error' });
  });
});
