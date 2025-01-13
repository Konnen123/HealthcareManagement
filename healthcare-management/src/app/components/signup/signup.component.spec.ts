import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { SignupComponent } from './signup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { LanguageService } from '../../services/language/language.service';
import { TranslateModule } from '@ngx-translate/core';
import { of } from 'rxjs';
import { Directive, Input, HostListener } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[routerLink]',
})
export class MockRouterLink {
  @Input() routerLink: any;

  @HostListener('click') onClick() {
    // Simulate click interaction
  }
}
// Mock RouterLink directive
import { ActivatedRoute } from '@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

// Mock ActivatedRoute
class MockActivatedRoute {
  snapshot = { paramMap: { get: () => null } }; // Provide a mock snapshot
}

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;
  let mockAuthService: jasmine.SpyObj<AuthenticationService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('AuthenticationService', ['registerAsync']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate'], {
      events: of({}), // Mock router events
      createUrlTree: jasmine.createSpy('createUrlTree').and.returnValue({}),
      serializeUrl: jasmine.createSpy('serializeUrl').and.returnValue('mock-url'),
    });
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage']);
    mockSnackBar = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        MatSnackBarModule,
        TranslateModule.forRoot(),
        SignupComponent, // Import the standalone component directly
        MockRouterLink, // Import the mock RouterLink directive
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AuthenticationService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }, // Provide the mocked Router with createUrlTree
        { provide: LanguageService, useValue: mockLanguageService },
        { provide: MatSnackBar, useValue: mockSnackBar },
        { provide: ActivatedRoute, useClass: MockActivatedRoute }, // Provide the mock ActivatedRoute
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });
  it('should initialize the form with default values', () => {
    expect(component.signupForm.value).toEqual({
      firstName: '',
      lastName: '',
      phoneNumber: '',
      dateOfBirth: '',
      email: '',
      role: '',
      password: '',
      confirmPassword: '',
    });
  });

  it('should mark the form as valid when all fields are filled correctly', () => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      phoneNumber: '1234567890',
      dateOfBirth: '2000-01-01',
      email: 'john.doe@example.com',
      role: 'user',
      password: 'password123',
      confirmPassword: 'password123',
    });
    expect(component.signupForm.valid).toBeTruthy();
  });

  it('should show an error if passwords do not match', () => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      phoneNumber: '1234567890',
      dateOfBirth: '2000-01-01',
      email: 'john.doe@example.com',
      role: 'user',
      password: 'password123',
      confirmPassword: 'password456', // Password mismatch
    });
    const errors = component.signupForm.errors;
    expect(errors).toEqual({ mismatch: true });
  });

  it('should call registerAsync on valid form submission', fakeAsync(() => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      phoneNumber: '1234567890',
      dateOfBirth: '2000-01-01',
      email: 'john.doe@example.com',
      role: 'user',
      password: 'password123',
      confirmPassword: 'password123',
    });

    mockAuthService.registerAsync.and.returnValue(Promise.resolve());

    component.onSubmit();
    tick();

    expect(mockAuthService.registerAsync).toHaveBeenCalled();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Registration successful.Check your email to verify email account.',
      'Close',
      { duration: 5000, panelClass: ['error-snackbar'] }
    );
  }));

  it('should show a snackbar message on registration failure', fakeAsync(() => {
    component.signupForm.setValue({
      firstName: 'Jane',
      lastName: 'Doe',
      phoneNumber: '9876543210',
      dateOfBirth: '1995-12-12',
      email: 'jane.doe@example.com',
      role: 'admin',
      password: 'password456',
      confirmPassword: 'password456',
    });

    mockAuthService.registerAsync.and.returnValue(Promise.reject(new Error('Registration failed')));

    component.onSubmit();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Failed to register. Please try again.',
      'Close',
      { duration: 5000, panelClass: ['error-snackbar'] }
    );
  }));

  it('should call languageService.setLanguage on initialization', () => {
    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
  });

  it('should not call registerAsync if the form is invalid', fakeAsync(() => {
    component.signupForm.setValue({
      firstName: '',
      lastName: '',
      phoneNumber: '',
      dateOfBirth: '',
      email: '',
      role: '',
      password: '',
      confirmPassword: '',
    });

    component.onSubmit();
    tick();

    expect(mockAuthService.registerAsync).not.toHaveBeenCalled();
    expect(mockSnackBar.open).not.toHaveBeenCalled();
  }));

  it('should handle unexpected errors from registerAsync gracefully', fakeAsync(() => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      phoneNumber: '1234567890',
      dateOfBirth: '2000-01-01',
      email: 'john.doe@example.com',
      role: 'user',
      password: 'password123',
      confirmPassword: 'password123',
    });

    mockAuthService.registerAsync.and.returnValue(Promise.reject(new Error('Unexpected error')));

    component.onSubmit();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Failed to register. Please try again.',
      'Close',
      { duration: 5000, panelClass: ['error-snackbar'] }
    );
  }));

  it('should display an error message for invalid email', () => {
    const emailControl = component.signupForm.get('email');
    emailControl?.setValue('invalid-email');
    expect(emailControl?.invalid).toBeTrue();
    expect(emailControl?.errors?.['email']).toBeTrue();
  });


  it('should set the form-level validator correctly for mismatched passwords', () => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      phoneNumber: '1234567890',
      dateOfBirth: '2000-01-01',
      email: 'john.doe@example.com',
      role: 'user',
      password: 'password123',
      confirmPassword: 'differentPassword',
    });

    expect(component.signupForm.errors).toEqual({ mismatch: true });
  });

});
