import { TestBed } from '@angular/core/testing';
import { SymptomClient } from './symptom.client';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { APP_SERVICE_CONFIG } from '../../app-config/app.config';
import { AppConfig } from '../../app-config/app.config.interface';

describe('SymptomClient', () => {
  let client: SymptomClient;
  let httpMock: HttpTestingController;
  const mockConfig: AppConfig = {apiEndpoint: 'http://localhost/api'};

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule], // Include the HttpClientTestingModule
      providers: [
        SymptomClient,
        {provide: APP_SERVICE_CONFIG, useValue: mockConfig},
      ],
    });

    client = TestBed.inject(SymptomClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no unmatched requests
  });

  it('should be created', () => {
    expect(client).toBeTruthy();
  });

  it('should make a POST request to predict endpoint', () => {
    const symptoms = {symptom1: true, symptom2: false};
    const mockResponse = {prediction: 'Disease X'};

    client.predict(symptoms).subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/DiseasePrediction/predict`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(symptoms);
    req.flush(mockResponse); // Simulate the response
  });

  it('should handle error in POST request', () => {
    const symptoms = { symptom1: true, symptom2: false };

    client.predict(symptoms).subscribe({
      next: () => fail('Should have failed with 500 status'),
      error: (error) => {
        expect(error.status).toBe(500);
      },
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/DiseasePrediction/predict`);
    expect(req.request.method).toBe('POST');
    req.flush('Server Error', { status: 500, statusText: 'Internal Server Error' });
  });

  it('should handle an empty symptoms object gracefully', () => {
    const symptoms = {};
    const mockResponse = { prediction: 'No disease detected' };

    client.predict(symptoms).subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/DiseasePrediction/predict`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(symptoms);
    req.flush(mockResponse); // Simulate the response
  });

  it('should handle a large symptoms list', () => {
    const symptoms = Array(100).fill({ symptom: true });
    const mockResponse = { prediction: 'Disease X' };

    client.predict(symptoms).subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${mockConfig.apiEndpoint}/v1/DiseasePrediction/predict`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(symptoms);
    req.flush(mockResponse); // Simulate the response
  });

});
