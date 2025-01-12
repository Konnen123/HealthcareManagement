import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SymptomCheckerComponent } from './symptom-checker.component';
import {EventEmitter} from '@angular/core';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {of} from 'rxjs';
import {LanguageService} from '../../services/language/language.service';
import {SymptomService} from '../../services/symptom-checker/symptom-checker.service';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

describe('SymptomCheckerComponent', () => {
  let component: SymptomCheckerComponent;
  let fixture: ComponentFixture<SymptomCheckerComponent>;

  beforeEach(async () => {
    const symptomService = jasmine.createSpyObj('SymptomService', ['getSymptoms']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage', 'getLanguage']);
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
      imports: [
        SymptomCheckerComponent,
      BrowserAnimationsModule],
      providers: [
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock },
        { provide: SymptomService, useValue: symptomService}
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SymptomCheckerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
