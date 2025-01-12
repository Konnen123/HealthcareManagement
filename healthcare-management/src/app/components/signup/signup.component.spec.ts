import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupComponent } from './signup.component';
import {EventEmitter} from '@angular/core';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {of} from 'rxjs';
import {LanguageService} from '../../services/language/language.service';
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;

  beforeEach(async () => {
    const authenticationServiceSpy = jasmine.createSpyObj('AuthenticationService', ['registerAsync']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);
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
        SignupComponent,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AuthenticationService, useValue: authenticationServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock}
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
