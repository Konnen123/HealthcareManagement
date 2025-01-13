import { ComponentFixture, TestBed } from '@angular/core/testing';
import { VerifyEmailComponent } from './verify-email.component';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { of } from 'rxjs';

describe('VerifyEmailComponent', () => {
  let component: VerifyEmailComponent;
  let fixture: ComponentFixture<VerifyEmailComponent>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockActivatedRoute: Partial<ActivatedRoute>;

  beforeEach(async () => {
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    mockActivatedRoute = {
      queryParams: of({ status: 'success', description: 'verification_successful' })
    };

    await TestBed.configureTestingModule({
      imports: [
        VerifyEmailComponent,
        MatCardModule,
        MatButtonModule
      ],
      providers: [
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(VerifyEmailComponent);
    component = fixture.componentInstance;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set success message for a successful verification', () => {
    component.ngOnInit();

    expect(component.status).toBe('success');
    expect(component.isSuccess).toBeTrue();
    expect(component.message).toBe('Your email has been successfully verified! 🎉');
  });

  it('should set error message for verification_failed', () => {
    mockActivatedRoute.queryParams = of({ status: 'error', description: 'verification_failed' });
    component.ngOnInit();

    expect(component.status).toBe('error');
    expect(component.isSuccess).toBeFalse();
    expect(component.message).toBe('The email verification failed. Please try again. 💔');
  });

  it('should set error message for token_expired', () => {
    mockActivatedRoute.queryParams = of({ status: 'error', description: 'token_expired' });
    component.ngOnInit();

    expect(component.status).toBe('error');
    expect(component.isSuccess).toBeFalse();
    expect(component.message).toBe('The verification token has expired. Please request a new verification email. ⌛');
  });

  it('should set error message for token_not_found', () => {
    mockActivatedRoute.queryParams = of({ status: 'error', description: 'token_not_found' });
    component.ngOnInit();

    expect(component.status).toBe('error');
    expect(component.isSuccess).toBeFalse();
    expect(component.message).toBe('The verification token is invalid or missing. Please check your email link. 🚫');
  });

  it('should set default error message for unknown error description', () => {
    mockActivatedRoute.queryParams = of({ status: 'error', description: 'unknown_error' });
    component.ngOnInit();

    expect(component.status).toBe('error');
    expect(component.isSuccess).toBeFalse();
    expect(component.message).toBe('An unknown error occurred during email verification. Please contact support. 🛠️');
  });

  it('should set message for invalid request if status is missing', () => {
    mockActivatedRoute.queryParams = of({});
    component.ngOnInit();

    expect(component.message).toBe('Invalid request. Please check the link or contact support. ❓');
  });

  it('should navigate to homepage on goToHomepage', () => {
    component.goToHomepage();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/']);
  });
});
