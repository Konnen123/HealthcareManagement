import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { LanguageService } from '../../services/language/language.service';
import { TranslateModule, TranslatePipe } from '@ngx-translate/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EventEmitter } from '@angular/core';
import { of } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockAuthenticationService: jasmine.SpyObj<AuthenticationService>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockAuthenticationService = jasmine.createSpyObj('AuthenticationService', [
      'loginAsync',
      'setCookie',
    ]);
    mockSnackBar = jasmine.createSpyObj('MatSnackBar', ['open']);
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    await TestBed.configureTestingModule({
      imports: [
        LoginComponent,
        ReactiveFormsModule,
        FormsModule,
        MatSnackBarModule,
        MatInputModule,
        MatFormFieldModule,
        MatButtonModule,
        TranslateModule.forRoot(),
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: AuthenticationService, useValue: mockAuthenticationService },
        { provide: MatSnackBar, useValue: mockSnackBar },
        { provide: LanguageService, useValue: mockLanguageService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call setLanguage on initialization', () => {
    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
  });

  it('should initialize the form with email and password controls', () => {
    expect(component.loginForm.contains('email')).toBeTrue();
    expect(component.loginForm.contains('password')).toBeTrue();
  });

  it('should make email and password controls required', () => {
    const emailControl = component.loginForm.get('email');
    const passwordControl = component.loginForm.get('password');

    emailControl?.setValue('');
    passwordControl?.setValue('');

    expect(emailControl?.valid).toBeFalse();
    expect(passwordControl?.valid).toBeFalse();
  });

  it('should call loginAsync and set cookies on successful login', async () => {
    const mockResponse = {
      accessToken: 'mockAccessToken',
      refreshToken: 'mockRefreshToken',
    };
    mockAuthenticationService.loginAsync.and.returnValue(Promise.resolve(mockResponse));

    component.loginForm.setValue({
      email: 'test@example.com',
      password: 'password123',
    });

    await component.onSubmit();

    expect(mockAuthenticationService.loginAsync).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: 'password123',
    });
    expect(mockAuthenticationService.setCookie).toHaveBeenCalledWith(
      'token',
      'mockAccessToken'
    );
    expect(mockAuthenticationService.setCookie).toHaveBeenCalledWith(
      'refreshToken',
      'mockRefreshToken'
    );
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should navigate to signup page when redirectToSignup is called', () => {
    component.redirectToSignup();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/signup']);
  });

  it('should navigate to forgot-password page when redirectToForgotPassword is called', () => {
    component.redirectToForgotPassword();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/forgot-password']);
  });
});
