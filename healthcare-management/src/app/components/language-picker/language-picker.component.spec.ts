import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LanguagePickerComponent } from './language-picker.component';
import {EventEmitter} from '@angular/core';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {of} from 'rxjs';
import {LanguageService} from '../../services/language/language.service';

describe('LanguagePickerComponent', () => {
  let component: LanguagePickerComponent;
  let fixture: ComponentFixture<LanguagePickerComponent>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['getLanguages', 'changeLanguage', 'getLanguage']);
    const translateService = jasmine.createSpyObj('TranslateService', ['instant', 'get']);
    const translateServiceMock = {
      currentLang: 'ro',
      onLangChange: new EventEmitter<LangChangeEvent>(),
      use: translateService.get,
      get: translateService.get.and.returnValue(of('')),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter()
    }
    await TestBed.configureTestingModule({
      imports: [LanguagePickerComponent],
      providers: [
        { provide: LanguageService, useValue: mockLanguageService  },
        { provide: TranslateService, useValue: translateServiceMock}
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LanguagePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize languages from the language service on ngOnInit', () => {
    const mockLanguages = new Map<string, string>([
      ['en', 'English'],
      ['ro', 'Română']
    ]);
    mockLanguageService.getLanguages.and.returnValue(mockLanguages);

    component.ngOnInit();

    expect(component.languages).toEqual(mockLanguages);
    expect(mockLanguageService.getLanguages).toHaveBeenCalled();
  });

  it('should return true if the selected language matches the current language', () => {
    mockLanguageService.getLanguage.and.returnValue('ro');

    const result = component.isLanguageSelected('ro');

    expect(result).toBeTrue();
    expect(mockLanguageService.getLanguage).toHaveBeenCalled();
  });

  it('should return false if the selected language does not match the current language', () => {
    mockLanguageService.getLanguage.and.returnValue('en');

    const result = component.isLanguageSelected('ro');

    expect(result).toBeFalse();
    expect(mockLanguageService.getLanguage).toHaveBeenCalled();
  });

});
