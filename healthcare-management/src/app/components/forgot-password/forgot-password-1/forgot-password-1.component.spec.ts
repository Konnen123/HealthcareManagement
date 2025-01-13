import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ForgotPassword1Component } from './forgot-password-1.component';
import { Router } from '@angular/router';
import { MailService } from '../../../services/mail/mail.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { LanguageService } from '../../../services/language/language.service';
import { TranslateService } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { EventEmitter } from '@angular/core';

describe('ForgotPassword1Component', () => {
  let component: ForgotPassword1Component;
  let fixture: ComponentFixture<ForgotPassword1Component>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockMailService: jasmine.SpyObj<MailService>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const mailServiceSpy = jasmine.createSpyObj('MailService', ['sendForgotPasswordEmailAsync']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'ro',
      onLangChange: new EventEmitter(),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter(),
    };

    await TestBed.configureTestingModule({
      imports: [
        ForgotPassword1Component,
        MatSnackBarModule,
        ReactiveFormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: Router, useValue: routerSpy },
        { provide: MailService, useValue: mailServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock },
        FormBuilder,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ForgotPassword1Component);
    component = fixture.componentInstance;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    mockMailService = TestBed.inject(MailService) as jasmine.SpyObj<MailService>;
    mockLanguageService = TestBed.inject(LanguageService) as jasmine.SpyObj<LanguageService>;

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    const emailControl = component.forgotPasswordForm.get('email');
    expect(emailControl).toBeTruthy();
    expect(emailControl?.value).toBe('');
    expect(emailControl?.valid).toBeFalse();
  });

  it('should call setLanguage on initialization', () => {
    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
  });

  it('should navigate to login page on redirectToLogin', () => {
    component.redirectToLogin();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should call sendForgotPasswordEmailAsync and show success snackbar on valid form submission', async () => {
    const email = 'test@example.com';
    const snackBarSpy = spyOn(component.snackBar, 'open');

    mockMailService.sendForgotPasswordEmailAsync.and.returnValue(Promise.resolve());

    component.forgotPasswordForm.setValue({ email });
    await component.onSubmit();

    expect(mockMailService.sendForgotPasswordEmailAsync).toHaveBeenCalledWith(email);
    expect(snackBarSpy).toHaveBeenCalledWith('Please check your email to get the reset password link.', 'Close', { duration: 5000 });
  });


  it('should not call sendForgotPasswordEmailAsync if form is invalid', async () => {
    component.forgotPasswordForm.setValue({ email: '' }); // Invalid form
    await component.onSubmit();

    expect(mockMailService.sendForgotPasswordEmailAsync).not.toHaveBeenCalled();
  });
});
