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
});
