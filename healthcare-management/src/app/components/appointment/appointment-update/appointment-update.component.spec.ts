import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentUpdateComponent } from './appointment-update.component';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AppointmentFormComponent } from '../appointment-form/appointment-form.component';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LanguageService } from '../../../services/language/language.service';
import { TranslateService } from '@ngx-translate/core';
import { of } from 'rxjs';
import { EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user/user.service';
import {Appointment} from '../../../models/appointment.model';

describe('AppointmentUpdateComponent', () => {
  let component: AppointmentUpdateComponent;
  let fixture: ComponentFixture<AppointmentUpdateComponent>;
  let mockAppointmentService: jasmine.SpyObj<AppointmentService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockUserService: jasmine.SpyObj<UserService>;

  beforeEach(async () => {
    const appointmentServiceSpy = jasmine.createSpyObj('AppointmentService', ['getByIdAsync', 'updateAsync']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);
    const userServiceSpy = jasmine.createSpyObj('UserService', ['getUsersAsync']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'ro',
      onLangChange: new EventEmitter(),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter(),
    };

    const activatedRouteMock = {
      snapshot: {
        paramMap: {
          get: (key: string) => (key === 'id' ? 'test-id' : null),
        },
      },
    };

    const appConfigMock = {
      apiEndpoint: 'https://mock-api.com',
    };

    await TestBed.configureTestingModule({
      imports: [
        AppointmentUpdateComponent,
        AppointmentFormComponent,
        MatSnackBarModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AppointmentService, useValue: appointmentServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock },
        { provide: UserService, useValue: userServiceSpy },
        { provide: ActivatedRoute, useValue: activatedRouteMock },
        { provide: 'app.config', useValue: appConfigMock }, // Mocked app.config provider
        provideNativeDateAdapter()
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentUpdateComponent);
    component = fixture.componentInstance;
    mockAppointmentService = TestBed.inject(AppointmentService) as jasmine.SpyObj<AppointmentService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    mockUserService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call getByIdAsync on init', async () => {
    const appointmentData: Appointment = {
      id: 'test-id',
      patientId: 'patient-123',
      date: new Date('2024-01-01'),
      startTime: '10:00',
      endTime: '11:00',
      doctorId: 'doctor-456',
    };
    mockAppointmentService.getByIdAsync.and.returnValue(Promise.resolve(appointmentData));

    await component.ngOnInit();

    expect(mockAppointmentService.getByIdAsync).toHaveBeenCalledWith('test-id');
    expect(component.appointmentData).toEqual(appointmentData);
  });

  it('should call updateAsync and show success snackbar on form submission', async () => {
    const appointmentData: Appointment = {
      id: 'test-id',
      patientId: 'patient-123',
      date: new Date('2024-01-01'),
      startTime: '10:00',
      endTime: '11:00',
      doctorId: 'doctor-456',
    };
    mockAppointmentService.updateAsync.and.returnValue(Promise.resolve(appointmentData));
    component.appointmentData = { id: 'test-id' };

    const mockFormData = {
      patientId: '123',
      doctorId: '456',
      date: new Date(),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Updated notes',
    };

    await component.onFormSubmit(mockFormData);

    expect(mockAppointmentService.updateAsync).toHaveBeenCalledWith({ ...mockFormData, id: 'test-id' });
  });

});
