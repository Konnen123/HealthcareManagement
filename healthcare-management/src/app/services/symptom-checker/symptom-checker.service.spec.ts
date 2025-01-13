import { TestBed } from '@angular/core/testing';
import { SymptomService } from './symptom-checker.service';
import { SymptomClient } from '../../clients/symptom/symptom.client';
import { of } from 'rxjs';
import { PLATFORM_ID } from '@angular/core';

describe('SymptomService', () => {
  let service: SymptomService;
  let mockSymptomClient: jasmine.SpyObj<SymptomClient>;

  beforeEach(() => {
    mockSymptomClient = jasmine.createSpyObj('SymptomClient', ['predict']);

    TestBed.configureTestingModule({
      providers: [
        SymptomService,
        { provide: SymptomClient, useValue: mockSymptomClient },
        { provide: PLATFORM_ID, useValue: 'browser' } // Setăm PLATFORM_ID pentru a simula browser-ul
      ],
    });

    service = TestBed.inject(SymptomService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call SymptomClient.predict and return its result in predictAsync', async () => {
    const mockSymptomsList = { symptoms: ['headache', 'fever'] };
    const mockResponse = { diagnosis: 'Flu' };

    mockSymptomClient.predict.and.returnValue(of(mockResponse)); // Simulăm răspunsul clientului

    const result = await service.predictAsync(mockSymptomsList);

    expect(mockSymptomClient.predict).toHaveBeenCalledWith(mockSymptomsList); // Verificăm apelul clientului
    expect(result).toEqual(mockResponse); // Verificăm rezultatul
  });
});
