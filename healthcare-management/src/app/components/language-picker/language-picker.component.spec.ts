import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LanguagePickerComponent } from './language-picker.component';
import {EventEmitter} from '@angular/core';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {of} from 'rxjs';
import {LanguageService} from '../../services/language/language.service';

describe('LanguagePickerComponent', () => {
  let component: LanguagePickerComponent;
  let fixture: ComponentFixture<LanguagePickerComponent>;

  beforeEach(async () => {
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['getLanguages', 'changeLanguage', 'getLanguage']);
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
        { provide: LanguageService, useValue: languageServiceSpy },
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
});
