import { TestBed } from '@angular/core/testing';
import { LanguageService } from './language.service';
import { TranslateService } from '@ngx-translate/core';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

describe('LanguageService', () => {
  let service: LanguageService;
  let translateServiceSpy: jasmine.SpyObj<TranslateService>;

  beforeEach(() => {
    translateServiceSpy = jasmine.createSpyObj('TranslateService', ['use', 'instant', 'get'], { currentLang: 'en' });

    TestBed.configureTestingModule({
      providers: [
        LanguageService,
        { provide: PLATFORM_ID, useValue: 'browser' }, // Simulăm rularea pe browser
        { provide: TranslateService, useValue: translateServiceSpy }, // Mock-ul TranslateService
      ],
    });

    service = TestBed.inject(LanguageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set the language and update document.lang', () => {
    spyOn(localStorage, 'getItem').and.returnValue('ro');
    spyOn(localStorage, 'setItem');

    service.setLanguage();

    expect(localStorage.setItem).toHaveBeenCalledWith('language', 'ro');
    expect(translateServiceSpy.use).toHaveBeenCalledWith('ro');
    expect(document.documentElement.lang).toBe('ro');
  });

  it('should return the current language', () => {
    const currentLang = service.getLanguage();
    expect(currentLang).toBe('en');
  });


  it('should return available languages', () => {
    const languages = service.getLanguages();
    expect(languages.size).toBe(2);
    expect(languages.get('en')).toBe('English');
    expect(languages.get('ro')).toBe('Română');
  });
});
