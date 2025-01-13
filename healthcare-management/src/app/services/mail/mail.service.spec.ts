import { TestBed } from '@angular/core/testing';
import { MailService } from './mail.service';
import { MailClient } from '../../clients/mail.client';
import { of } from 'rxjs';
import { PLATFORM_ID } from '@angular/core';

describe('MailService', () => {
  let service: MailService;
  let mockMailClient: jasmine.SpyObj<MailClient>;

  beforeEach(() => {
    mockMailClient = jasmine.createSpyObj('MailClient', ['sendForgotPasswordEmail']);

    TestBed.configureTestingModule({
      providers: [
        MailService,
        { provide: MailClient, useValue: mockMailClient },
        { provide: PLATFORM_ID, useValue: 'browser' }, // Simulăm rularea în browser
      ],
    });

    service = TestBed.inject(MailService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call MailClient.sendForgotPasswordEmail and return its result', async () => {
    const mockEmail = 'test@example.com';
    const mockResponse = { success: true };

    mockMailClient.sendForgotPasswordEmail.and.returnValue(of(mockResponse)); // Simulăm răspunsul clientului

    const result = await service.sendForgotPasswordEmailAsync(mockEmail);

    expect(mockMailClient.sendForgotPasswordEmail).toHaveBeenCalledWith(mockEmail); // Verificăm apelul
    expect(result).toEqual(mockResponse); // Verificăm rezultatul
  });
});
