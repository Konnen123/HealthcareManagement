import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentCreateComponent } from './appointment-create.component';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AppointmentFormComponent } from '../appointment-form/appointment-form.component';
import {MatInputModule} from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule, provideNativeDateAdapter} from '@angular/material/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
;

describe('AppointmentCreateComponent', () => {
  let component: AppointmentCreateComponent;
  let fixture: ComponentFixture<AppointmentCreateComponent>;
  let mockAppointmentService: jasmine.SpyObj<AppointmentService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockAppointmentService = jasmine.createSpyObj('AppointmentService', ['createAsync']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [
        AppointmentCreateComponent,
        AppointmentFormComponent,
        MatSnackBarModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AppointmentService, useValue: mockAppointmentService },
        { provide: Router, useValue: mockRouter },
        provideNativeDateAdapter()
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call createAsync and navigate on valid form submission', () => {
    mockAppointmentService.createAsync.and.returnValue(Promise.resolve({}));

    const mockFormData = {
      patientId: 'd99ccb79-67e3-4d7c-8725-14f01981448f',
      doctorId: 'd99ccb79-67e3-4d7c-8725-14f01981448f',
      date: new Date('2024-12-31'), // Use a Date object here
      startTime: '10:00',
      endTime: '11:00',
      userNotes: 'Test note',
    };
    component.onFormSubmit(mockFormData);

    expect(mockAppointmentService.createAsync).toHaveBeenCalledWith(mockFormData);
  });

});
