import { TestBed } from '@angular/core/testing';

import { LanguageService } from './language.service';

describe('LanguageService', () => {
  let service: LanguageService;

  beforeEach(() => {
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage', 'getLanguage']);
    const translateService = jasmine.createSpyObj('TranslateService', ['instant', 'get']);

    TestBed.configureTestingModule({
      providers: [
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: translateService, useValue: translateService}]
    });
    service = TestBed.inject(LanguageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
