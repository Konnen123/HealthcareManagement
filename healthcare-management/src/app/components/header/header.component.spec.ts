import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HeaderComponent } from './header.component';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LanguagePickerComponent } from '../language-picker/language-picker.component';
import { TranslatePipe, TranslateService, TranslateModule, TranslateStore } from '@ngx-translate/core';
import { NgIf } from '@angular/common';
import { of } from 'rxjs';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let mockAuthService: jasmine.SpyObj<AuthenticationService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getUserRole', 'logout']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'ro',
      onLangChange: of(),
      onTranslationChange: of(),
      onDefaultLangChange: of(),
    };

    await TestBed.configureTestingModule({
      imports: [
        MatToolbarModule,
        MatButtonModule,
        MatIconModule,
        MatTooltipModule,
        NgIf,
        TranslateModule.forRoot(), // Provides TranslateService and TranslateStore
        HeaderComponent, LanguagePickerComponent, TranslatePipe
      ],
      providers: [
        { provide: AuthenticationService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: TranslateService, useValue: translateServiceMock },
        TranslateStore
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    mockAuthService = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    mockAuthService.getUserRole.and.returnValue('admin');

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set userRole on initialization', () => {
    expect(mockAuthService.getUserRole).toHaveBeenCalled();
    expect(component.userRole).toBe('admin');
  });

  it('should call logout and navigate to /login on logout', () => {
    component.logout();

    expect(mockAuthService.logout).toHaveBeenCalled();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should navigate to /appointments on redirectToAppointments', () => {
    component.redirectToAppointments();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/appointments']);
  });

  it('should navigate to /symptom-checker on redirectToSymptomChecker', () => {
    component.redirectToSymptomChecker();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/symptom-checker']);
  });

  it('should navigate to /appointments/create on redirectToCreateAppointment', () => {
    component.redirectToCreateAppointment();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/appointments/create']);
  });

  it('should navigate to /home on redirectToHomepage', () => {
    component.redirectToHomepage();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should navigate to / on onLogoClicked', () => {
    component.onLogoClicked();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/']);
  });
});
