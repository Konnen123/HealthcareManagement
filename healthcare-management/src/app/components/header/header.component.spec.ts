import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderComponent } from './header.component';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {EventEmitter} from '@angular/core';
import {of} from 'rxjs';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;


  beforeEach(async () => {
    const authenticationServiceSpy = jasmine.createSpyObj('AuthenticationService', ['logout']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const translateService = jasmine.createSpyObj<TranslateService>('translateService', ['instant', 'get']);
    const translateServiceMock = {
      currentLang: 'ro',
      onLangChange: new EventEmitter<LangChangeEvent>(),
      use: translateService.get,
      get: translateService.get.and.returnValue(of('')),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter()
    };

    await TestBed.configureTestingModule({
      imports: [HeaderComponent],
      providers: [
        { provide: AuthenticationService, useValue: authenticationServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: TranslateService, useValue: translateServiceMock },
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
