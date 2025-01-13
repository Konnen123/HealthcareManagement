import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ForgotPassword2Component } from './forgot-password-2.component';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
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

describe('ForgotPassword2Component', () => {
  let component: ForgotPassword2Component;
  let fixture: ComponentFixture<ForgotPassword2Component>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockAuthService: jasmine.SpyObj<AuthenticationService>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;
  let mockActivatedRoute: Partial<ActivatedRoute>;

  beforeEach(async () => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['resetPasswordAsync']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'ro',
      onLangChange: new EventEmitter(),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter(),
    };

    mockActivatedRoute = {
      queryParams: of({ token: 'test-token' }),
    };

    await TestBed.configureTestingModule({
      imports: [
        ForgotPassword2Component,
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
        { provide: AuthenticationService, useValue: authServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: TranslateService, useValue: translateServiceMock },
        FormBuilder,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ForgotPassword2Component);
    component = fixture.componentInstance;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    mockAuthService = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
    mockLanguageService = TestBed.inject(LanguageService) as jasmine.SpyObj<LanguageService>;

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    const newPasswordControl = component.resetPasswordForm.get('newPassword');
    const confirmPasswordControl = component.resetPasswordForm.get('confirmPassword');

    expect(newPasswordControl).toBeTruthy();
    expect(newPasswordControl?.value).toBe('');
    expect(confirmPasswordControl).toBeTruthy();
    expect(confirmPasswordControl?.value).toBe('');
  });

  it('should set token from query parameters on init', () => {
    expect(component.token).toBe('test-token');
  });

  it('should navigate to login page and show success snackbar on successful submission', async () => {
    const snackBarSpy = spyOn(component.snackBar, 'open');

    mockAuthService.resetPasswordAsync.and.returnValue(Promise.resolve());
    component.token = 'test-token';

    component.resetPasswordForm.setValue({
      newPassword: 'NewPassword123',
      confirmPassword: 'NewPassword123'
    });

    await component.onSubmit();

    expect(mockAuthService.resetPasswordAsync).toHaveBeenCalledWith({
      password: 'NewPassword123',
      token: 'test-token'
    });
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
    expect(snackBarSpy).toHaveBeenCalledWith('Password reset successfully', 'Close', {
      duration: 3000
    });
  });


  it('should invalidate form if passwords do not match', () => {
    component.resetPasswordForm.setValue({
      newPassword: 'NewPassword123',
      confirmPassword: 'DifferentPassword'
    });

    const confirmPasswordControl = component.resetPasswordForm.get('confirmPassword');
    expect(confirmPasswordControl?.errors).toEqual({ passwordsMismatch: true });
  });
});
