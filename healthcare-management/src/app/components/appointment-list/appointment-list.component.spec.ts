
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentListComponent } from './appointment-list.component';
import { AppointmentService } from '../../services/appointment/appointment.service';
import { of } from 'rxjs';
import { Appointment } from '../../models/appointment.model';
import {AppointmentDetailComponent} from '../appointment-detail/appointment-detail.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {Router} from '@angular/router';
import {InjectionToken} from '@angular/core';

const APP_CONFIG = new InjectionToken<any>('app.config');

describe('AppointmentListComponent', () => {
  let component: AppointmentListComponent;
  let fixture: ComponentFixture<AppointmentListComponent>;
  const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
  let appointmentService: jasmine.SpyObj<AppointmentService>;

  beforeEach(async () => {
    const appointmentServiceSpy = jasmine.createSpyObj('AppointmentService', ['getAllAsync']);
    await TestBed.configureTestingModule({
      imports: [
        AppointmentDetailComponent,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: AppointmentService, useValue: appointmentServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: APP_CONFIG, useValue: {} },
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppointmentListComponent);
    component = fixture.componentInstance;
    appointmentService = TestBed.inject(AppointmentService) as jasmine.SpyObj<AppointmentService>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // it('should fetch appointments and set appointments property', async () => {
  //   const mockAppointments: Appointment[] = [
  //     {
  //       id: '1',
  //       patientId: '123e4567-e89b-12d3-a456-426614174000',
  //       doctorId: '123e4567-e89b-12d3-a456-426614174000',
  //       date: new Date('2024-12-01'),
  //       startTime: '10:00',
  //       endTime: '11:00',
  //       userNotes: 'Test notes 1'
  //     },
  //     {
  //       id: '2',
  //       patientId: '123e4567-e89b-12d3-a456-426614174001',
  //       doctorId: '123e4567-e89b-12d3-a456-426614174001',
  //       date: new Date('2024-12-02'),
  //       startTime: '12:00',
  //       endTime: '13:00',
  //       userNotes: 'Test notes 2'
  //     }
  //   ];
  //
  //   appointmentService.getAllAsync.and.returnValue(Promise.resolve(mockAppointments));
  //   component.fetchAppointments();
  //
  //   await fixture.whenStable();
  //
  //   expect(appointmentService.getAllAsync).toHaveBeenCalled();
  //   expect(component.appointments).toEqual(mockAppointments);
  // });
});


