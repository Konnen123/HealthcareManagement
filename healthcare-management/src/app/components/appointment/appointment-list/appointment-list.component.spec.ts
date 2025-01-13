import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppointmentListComponent } from './appointment-list.component';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LanguageService } from '../../../services/language/language.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import {Appointment} from '../../../models/appointment.model';

describe('AppointmentListComponent', () => {
  let component: AppointmentListComponent;
  let fixture: ComponentFixture<AppointmentListComponent>;
  let mockAppointmentService: jasmine.SpyObj<AppointmentService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    mockAppointmentService = jasmine.createSpyObj('AppointmentService', [
      'getAppointmentsPaginatedAsync',
      'getAllAsync',
    ]);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    await TestBed.configureTestingModule({
      imports: [
        AppointmentListComponent,
        MatSnackBarModule,
        MatDialogModule,
        MatTableModule,
        MatPaginatorModule,
        MatIconModule,
        MatButtonModule,
        TranslateModule.forRoot(),
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: AppointmentService, useValue: mockAppointmentService },
        { provide: Router, useValue: mockRouter },
        { provide: LanguageService, useValue: mockLanguageService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentListComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call fetchAppointments and initialize page on ngOnInit', () => {
    spyOn(component, 'fetchAppointments');
    spyOn(component, 'initializePage');

    component.ngOnInit();

    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
    expect(component.fetchAppointments).toHaveBeenCalled();
    expect(component.initializePage).toHaveBeenCalled();
  });

  it('should load appointments on handlePageEvent', () => {
    const mockPageEvent = { pageIndex: 1, pageSize: 5 } as any;
    mockAppointmentService.getAppointmentsPaginatedAsync.and.returnValue(Promise.resolve([]));

    component.handlePageEvent(mockPageEvent);

    expect(component.appointmentParams.top).toBe(5);
    expect(component.appointmentParams.skip).toBe(5);
    expect(mockAppointmentService.getAppointmentsPaginatedAsync).toHaveBeenCalled();
  });

  it('should fetch total appointments count on fetchAppointments', async () => {
    const mockAppointments: Appointment[] = [
      {
        id: '1',
        patientId: 'patient-1',
        doctorId: 'doctor-1',
        date: new Date('2024-12-31'),
        startTime: '10:00',
        endTime: '11:00',
      },
      {
        id: '2',
        patientId: 'patient-2',
        doctorId: 'doctor-2',
        date: new Date('2024-12-31'),
        startTime: '12:00',
        endTime: '13:00',
      },
    ];
    mockAppointmentService.getAllAsync.and.returnValue(Promise.resolve(mockAppointments));

    await component.fetchAppointments();

    expect(component.totalCountAppointments).toBe(mockAppointments.length);
    expect(mockAppointmentService.getAllAsync).toHaveBeenCalledWith(
      component.appointmentParams.startTime,
      component.appointmentParams.date
    );
  });

  it('should open the filters dialog and reload appointments', () => {
    spyOn(component.dialogService, 'open').and.returnValue({
      afterClosed: () => of({ date: new Date(), startTime: '10:00' }),
    } as any);
    spyOn(component, 'fetchAppointments');
    spyOn(component, 'loadAppointments');

    component.openFiltersDialog();

    expect(component.fetchAppointments).toHaveBeenCalled();
    expect(component.loadAppointments).toHaveBeenCalled();
  });

  it('should navigate to appointment details when a row is clicked', () => {
    const mockRow = { id: '123' };
    component.onAppointmentClicked(mockRow);

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/appointments', '123']);
  });

  it('should navigate to update appointment page', () => {
    component.updateAppointment('123');

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/appointments/update/', '123']);
  });

  it('should initialize displayedColumns correctly', () => {
    expect(component.displayedColumns).toEqual(['patientId', 'doctorId', 'date', 'time']);
  });

  it('should initialize currentPage correctly', () => {
    expect(component.currentPage).toBe(0);
  });

  it('should open filters dialog and reset pagination', () => {
    const mockFilterResult = { date: new Date('2024-12-31'), startTime: '10:00' };
    spyOn(component.dialogService, 'open').and.returnValue({
      afterClosed: () => of(mockFilterResult),
    } as any);

    spyOn(component, 'fetchAppointments');
    spyOn(component, 'loadAppointments');

    component.openFiltersDialog();

    expect(component.appointmentParams.date).toBe('2024-12-31');
    expect(component.appointmentParams.startTime).toBe('10:00');
    expect(component.currentPage).toBe(0);
    expect(component.fetchAppointments).toHaveBeenCalled();
    expect(component.loadAppointments).toHaveBeenCalled();
  });

  it('should call showErrorSnackbar with correct arguments', () => {
    spyOn(component.snackBar, 'open');
    const errorMessage = 'Test error message';

    component.showErrorSnackbar(errorMessage);

    expect(component.snackBar.open).toHaveBeenCalledWith(errorMessage, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['error-snackbar'],
    });
  });

  it('should reset appointmentParams when filters are applied', () => {
    spyOn(component, 'fetchAppointments');
    spyOn(component, 'loadAppointments');

    const mockFilterResult = { date: new Date('2024-12-31'), startTime: '10:00' };
    spyOn(component.dialogService, 'open').and.returnValue({
      afterClosed: () => of(mockFilterResult),
    } as any);

    component.openFiltersDialog();

    expect(component.appointmentParams.date).toBe('2024-12-31');
    expect(component.appointmentParams.startTime).toBe('10:00');
    expect(component.appointmentParams.skip).toBe(0);
    expect(component.currentPage).toBe(0);
    expect(component.fetchAppointments).toHaveBeenCalled();
    expect(component.loadAppointments).toHaveBeenCalled();
  });

  it('should handle empty filters result correctly', () => {
    spyOn(component.dialogService, 'open').and.returnValue({
      afterClosed: () => of(null),
    } as any);

    spyOn(component, 'fetchAppointments');
    spyOn(component, 'loadAppointments');

    component.openFiltersDialog();

    expect(component.fetchAppointments).not.toHaveBeenCalled();
    expect(component.loadAppointments).not.toHaveBeenCalled();
  });

  it('should handle no appointments returned by fetchAppointments', async () => {
    mockAppointmentService.getAllAsync.and.returnValue(Promise.resolve([])); // Simulate no appointments

    await component.fetchAppointments(); // Call the method

    expect(component.totalCountAppointments).toBe(0); // Validate total count is updated
    expect(component.appointments).toBeUndefined(); // Ensure appointments array remains unchanged
  });


  it('should handle no appointments returned by loadAppointments', async () => {
    mockAppointmentService.getAppointmentsPaginatedAsync.and.returnValue(Promise.resolve([]));

    await component.loadAppointments();

    expect(component.appointments).toEqual([]);
  });

  it('should set default page size and page options', () => {
    expect(component.pageSize).toBe(10);
    expect(component.pageSizeOptions).toEqual([5, 10, 20]);
  });
});
