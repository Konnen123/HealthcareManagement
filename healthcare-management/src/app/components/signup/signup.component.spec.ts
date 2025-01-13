import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SignupComponent } from './signup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { LanguageService } from '../../services/language/language.service';
import { TranslateModule } from '@ngx-translate/core';
import { Directive, Input } from '@angular/core';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

// Mock RouterLink directive
@Directive({
  selector: '[routerLink]',
})
class MockRouterLink {
  @Input() routerLink: any;
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
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage']);
    mockSnackBar = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        MatSnackBarModule,
        TranslateModule.forRoot(),
        SignupComponent,
        MockRouterLink
      ],
      providers: [
        { provide: AuthenticationService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
        { provide: LanguageService, useValue: mockLanguageService },
        { provide: MatSnackBar, useValue: mockSnackBar },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => null } } },
        }, // Mock ActivatedRoute
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });
  // Add your other tests here
});
