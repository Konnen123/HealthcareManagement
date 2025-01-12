import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import {APP_CONFIG} from './app-config/app.config';
import {ActivatedRoute} from '@angular/router';
import {AuthenticationService} from './services/authentication/authentication.service';
import {EventEmitter} from '@angular/core';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {of} from 'rxjs';
import {LanguageService} from './services/language/language.service';

describe('AppComponent', () => {
  beforeEach(async () => {
    const activatedRouteSpy = jasmine.createSpyObj('ActivatedRoute', ['snapshot']);
    const authenticationServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getUserRole', 'isAuthenticated']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage', 'getLanguage', 'getLanguages']);
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
      imports: [AppComponent],
      providers: [
        { provide: APP_CONFIG, useValue: {} },
        { provide: ActivatedRoute, useValue: activatedRouteSpy },
        { provide: AuthenticationService, useValue: authenticationServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock}
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have the 'healthcare-management' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('healthcare-management');
  });

  it('should render router-outlet', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('router-outlet')).not.toBeNull();
  });
});
