import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HomePageComponent } from './home-page.component';
import { Router } from '@angular/router';
import { EventEmitter } from '@angular/core';
import { of } from 'rxjs';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../../services/language/language.service';
import { RoleService } from '../../services/role/role.service';

describe('HomePageComponent', () => {
  let component: HomePageComponent;
  let fixture: ComponentFixture<HomePageComponent>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockRoleService: jasmine.SpyObj<RoleService>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockRoleService = jasmine.createSpyObj('RoleService', ['isUserDoctor', 'isUserPatient']);
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'ro',
      onLangChange: new EventEmitter<LangChangeEvent>(),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter(),
    };

    await TestBed.configureTestingModule({
      imports: [HomePageComponent],
      providers: [
        { provide: RoleService, useValue: mockRoleService },
        { provide: LanguageService, useValue: mockLanguageService },
        { provide: Router, useValue: mockRouter },
        { provide: TranslateService, useValue: translateServiceMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(HomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call languageService.setLanguage on initialization', () => {
    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
  });

  it('should return true when isUserDoctor is called and RoleService returns true', () => {
    mockRoleService.isUserDoctor.and.returnValue(true);
    expect(component.isUserDoctor()).toBeTrue();
    expect(mockRoleService.isUserDoctor).toHaveBeenCalled();
  });

  it('should return false when isUserDoctor is called and RoleService returns false', () => {
    mockRoleService.isUserDoctor.and.returnValue(false);
    expect(component.isUserDoctor()).toBeFalse();
    expect(mockRoleService.isUserDoctor).toHaveBeenCalled();
  });

  it('should return true when isUserPatient is called and RoleService returns true', () => {
    mockRoleService.isUserPatient.and.returnValue(true);
    expect(component.isUserPatient()).toBeTrue();
    expect(mockRoleService.isUserPatient).toHaveBeenCalled();
  });

  it('should return false when isUserPatient is called and RoleService returns false', () => {
    mockRoleService.isUserPatient.and.returnValue(false);
    expect(component.isUserPatient()).toBeFalse();
    expect(mockRoleService.isUserPatient).toHaveBeenCalled();
  });

  it('should navigate to /appointments when onViewAppointments is called', () => {
    component.onViewAppointments();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/appointments']);
  });

  it('should navigate to /appointments/create when onCreateAppointments is called', () => {
    component.onCreateAppointments();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/appointments/create']);
  });
});
