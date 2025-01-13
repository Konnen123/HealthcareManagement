import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { AppointmentFormComponent } from './appointment-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatNativeDateModule, provideNativeDateAdapter} from '@angular/material/core';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {UserService} from '../../../services/user/user.service';
import {LanguageService} from '../../../services/language/language.service';
import {LangChangeEvent, TranslateModule, TranslateService} from '@ngx-translate/core';
import {EventEmitter} from '@angular/core';
import {of} from 'rxjs';
import {DoctorDto} from '../../../shared/dtos/doctor.dto';

describe('AppointmentFormComponent', () => {
  let component: AppointmentFormComponent;
  let fixture: ComponentFixture<AppointmentFormComponent>;

  beforeEach(async () => {

    const userSpy = jasmine.createSpyObj('UserService', ['getUsersAsync']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);
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
      imports: [
        AppointmentFormComponent,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        BrowserAnimationsModule,
        TranslateModule.forRoot(),
      ],
      providers: [
        provideNativeDateAdapter(),
        { provide: UserService, useValue: userSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty fields', () => {
    expect(component.appointmentForm.value).toEqual({
      doctorId: '',
      date: '',
      startTime: '',
      endTime: '',
      userNotes: '',
    });
  });

  it('should not emit formSubmit when form is invalid', () => {
    spyOn(component.formSubmit, 'emit');
    component.onSubmit();
    expect(component.formSubmit.emit).not.toHaveBeenCalled();
  });

  it('should update form fields and selectedDoctor when initialData is set', () => {
    const mockInitialData = {
      doctor: {
        userId: '123', // Adăugat userId conform structurii așteptate
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        phoneNumber: '1234567890',
        dateOfBirth: new Date('1980-01-01'),
        createdAt: new Date('2020-01-01'),
        role: 'doctor',
      },
      date: new Date('2024-01-01'),
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test note',
    };

    component.initialData = mockInitialData;
    component.ngOnChanges();

    expect(component.appointmentForm.value).toEqual({
      doctorId: '123', // Se așteaptă să fie userId
      date: mockInitialData.date,
      startTime: mockInitialData.startTime,
      endTime: mockInitialData.endTime,
      userNotes: mockInitialData.userNotes,
    });
    expect(component.selectedDoctor).toEqual(mockInitialData.doctor);
  });

  it('should mark doctorId as invalid if empty', () => {
    component.appointmentForm.controls['doctorId'].setValue('');
    expect(component.appointmentForm.controls['doctorId'].valid).toBeFalse();
    expect(component.appointmentForm.controls['doctorId'].errors?.['required']).toBeTrue();
  });

  it('should mark date as invalid if it is in the past', () => {
    const pastDate = new Date(Date.now() - 24 * 60 * 60 * 1000); // Data trecută
    const control = component.appointmentForm.controls['date'];

    control.setValue(pastDate);
    control.markAsTouched();

    expect(control.valid).toBeFalse();
    expect(control.errors?.['pastDate']).toBeTrue();
  });

  it('should mark endTime as invalid if it is before startTime', () => {
    component.appointmentForm.controls['startTime'].setValue('12:00');
    component.appointmentForm.controls['endTime'].setValue('11:00');

    component.appointmentForm.controls['endTime'].markAsTouched(); // Declanșează validarea

    expect(component.appointmentForm.controls['endTime'].valid).toBeFalse();
    expect(component.appointmentForm.controls['endTime'].errors?.['endTimeBeforeStartTime']).toBeTrue(); // Corectarea cheii
  });

});
