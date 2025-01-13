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
        { provide: PLATFORM_ID, useValue: 'browser' },
        { provide: TranslateService, useValue: translateServiceSpy },
      ],
    });

    service = TestBed.inject(LanguageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set the language from localStorage if available', () => {
    spyOn(localStorage, 'getItem').and.returnValue('ro');
    spyOn(localStorage, 'setItem');

    service.setLanguage();

    expect(localStorage.setItem).toHaveBeenCalledWith('language', 'ro');
    expect(translateServiceSpy.use).toHaveBeenCalledWith('ro');
    expect(document.documentElement.lang).toBe('ro');
  });

  it('should set the default language if localStorage does not have a value', () => {
    spyOn(localStorage, 'getItem').and.returnValue(null);
    spyOn(localStorage, 'setItem');

    service.setLanguage();

    expect(localStorage.setItem).toHaveBeenCalledWith('language', 'en');
    expect(translateServiceSpy.use).toHaveBeenCalledWith('en');
    expect(document.documentElement.lang).toBe('en');
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

  // it('should not set language when running on a non-browser platform', () => {
  //   TestBed.overrideProvider(PLATFORM_ID, { useValue: 'server' });
  //   service = TestBed.inject(LanguageService);
  //
  //   spyOn(localStorage, 'setItem');
  //   service.setLanguage();
  //
  //   expect(localStorage.setItem).not.toHaveBeenCalled();
  //   expect(translateServiceSpy.use).not.toHaveBeenCalled();
  // });
  //
  // it('should not change the language when running on a non-browser platform', () => {
  //   TestBed.overrideProvider(PLATFORM_ID, { useValue: 'server' });
  //   service = TestBed.inject(LanguageService);
  //
  //   spyOn(localStorage, 'setItem');
  //   spyOn(location, 'reload');
  //   service.changeLanguage('ro');
  //
  //   expect(localStorage.setItem).not.toHaveBeenCalled();
  //   expect(location.reload).not.toHaveBeenCalled();
  // });
  //
  // it('should handle invalid or empty language values gracefully', () => {
  //   spyOn(localStorage, 'setItem');
  //   service.changeLanguage('');
  //
  //   expect(localStorage.setItem).toHaveBeenCalledWith('language', '');
  //   expect(translateServiceSpy.use).not.toHaveBeenCalled();
  // });
});
