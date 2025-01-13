import { TestBed } from '@angular/core/testing';
import { MailClient } from './mail.client';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { APP_SERVICE_CONFIG } from '../../app-config/app.config';
import { AppConfig } from '../../app-config/app.config.interface';

describe('MailClient', () => {
  let client: MailClient;
  let httpMock: HttpTestingController;
  const mockConfig: AppConfig = { apiEndpoint: 'http://localhost/api' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        MailClient,
        { provide: APP_SERVICE_CONFIG, useValue: mockConfig },
      ],
    });

    client = TestBed.inject(MailClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(client).toBeTruthy();
  });

  it('should send forgot password email and return success message', () => {
    const email = 'test@example.com';
    const mockResponse = 'Password reset email sent successfully.';

    client.sendForgotPasswordEmail(email).subscribe((response) => {
      expect(response).toEqual({ message: mockResponse });
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Mail/forgot-password`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ email });
    req.flush(mockResponse);
  });

  it('should handle server error in forgot password email request', () => {
    const email = 'test@example.com';
    const mockError = {
      status: 500,
      statusText: 'Internal Server Error',
      error: 'Server is down',
    };

    client.sendForgotPasswordEmail(email).subscribe({
      next: () => fail('Should have failed with 500 status'),
      error: (error) => {
        expect(error.status).toBe(500);
        expect(error.error).toBe('Server is down');
      },
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Mail/forgot-password`);
    expect(req.request.method).toBe('POST');
    req.flush(mockError.error, { status: mockError.status, statusText: mockError.statusText });
  });

  it('should handle non-JSON error response gracefully', () => {
    const email = 'test@example.com';
    const mockError = {
      status: 400,
      statusText: 'Bad Request',
      error: 'Invalid email format',
    };

    client.sendForgotPasswordEmail(email).subscribe({
      next: () => fail('Should have failed with 400 status'),
      error: (error) => {
        expect(error.status).toBe(400);
        expect(error.error).toBe('Invalid email format');
      },
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Mail/forgot-password`);
    expect(req.request.method).toBe('POST');
    req.flush(mockError.error, { status: mockError.status, statusText: mockError.statusText });
  });

  it('should throw an error if email is empty', () => {
    const email = '';
    const mockResponse = 'Email cannot be empty.';

    client.sendForgotPasswordEmail(email).subscribe({
      next: () => fail('Should have failed due to empty email'),
      error: (error) => {
        expect(error.status).toBe(400);
        expect(error.error).toBe(mockResponse);
      },
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Mail/forgot-password`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse, { status: 400, statusText: 'Bad Request' });
  });

  it('should parse JSON error message if provided as a string', () => {
    const email = 'test@example.com';
    const mockError = {
      status: 400,
      statusText: 'Bad Request',
      error: JSON.stringify({ message: 'Invalid email address' }),
    };

    client.sendForgotPasswordEmail(email).subscribe({
      next: () => fail('Should have failed with 400 status'),
      error: (error) => {
        expect(error.message).toBe('Invalid email address');
      },
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/Mail/forgot-password`);
    expect(req.request.method).toBe('POST');
    req.flush(mockError.error, { status: mockError.status, statusText: mockError.statusText });
  });

});
