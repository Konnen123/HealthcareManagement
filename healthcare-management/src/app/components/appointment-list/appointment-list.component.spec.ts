
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


});


