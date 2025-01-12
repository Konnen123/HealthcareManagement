import {Component, inject, OnInit} from '@angular/core';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import {Appointment} from '../../../models/appointment.model';
import {MatSnackBar} from '@angular/material/snack-bar';
import {formatDate, formatTime } from '../../../shared/date-time.utils';

import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import {MatTooltip} from '@angular/material/tooltip';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {MatButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {FilterDialogComponent} from '../../filter-dialog/filter-dialog.component';
import {MatDialog} from '@angular/material/dialog';
import {AppointmentParams} from '../../../models/appointmentParams.model';
import {TranslatePipe} from '@ngx-translate/core';
import {LanguageService} from '../../../services/language/language.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-appointment-list',
  imports: [
    MatTable,
    MatColumnDef,
    MatHeaderCell,
    MatCell,
    MatHeaderRow,
    MatCellDef,
    MatHeaderCellDef,
    MatHeaderRowDef,
    MatRow,
    MatRowDef,
    MatTooltip,
    MatPaginator,
    MatButton,
    MatIcon,
    TranslatePipe
  ],
  templateUrl: './appointment-list.component.html',
  standalone: true,
  styleUrl: './appointment-list.component.scss'
})
export class AppointmentListComponent implements OnInit
{
  appointments!: Appointment[];
  totalCountAppointments: number = 0;
  pageSize: number = 10;
  currentPage: number = 0;
  pageSizeOptions: number[] = [5, 10, 20];
  displayedColumns: string[] = ['patientId', 'doctorId', 'date', 'time'];
  readonly dialogService = inject(MatDialog);
  appointmentParams = new AppointmentParams();


  constructor(
    readonly appointmentService: AppointmentService,
    readonly snackBar: MatSnackBar,
    readonly languageService: LanguageService,
    readonly router: Router
  ) {
  }

  ngOnInit(): void {
    this.languageService.setLanguage();
    this.fetchAppointments();
    this.initializePage();
  }

  initializePage(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.appointmentService.getAppointmentsPaginatedAsync(this.appointmentParams).then(appointments => {
      this.appointments = appointments;
      //console.log('Appointments:', this.appointments);
    }).catch(error => {
      //console.error('Error while fetching appointments:', error);
      this.showErrorSnackbar('Error while fetching appointments');
    });
  }

  handlePageEvent(event: PageEvent): void {
    this.currentPage = event.pageIndex;

    this.appointmentParams.top = event.pageSize;
    this.appointmentParams.skip = this.currentPage * this.appointmentParams.top;
    this.loadAppointments();
  }

  fetchAppointments(): void{
    this.appointmentService.getAllAsync(this.appointmentParams.startTime, this.appointmentParams.date).then(app => {
      this.totalCountAppointments = app.length;
    }).catch(error => {
      console.error('Error while fetching appointments:', error);
      this.showErrorSnackbar('Error while fetching appointments');
    })
  }

  private showErrorSnackbar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['error-snackbar'],
    });
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FilterDialogComponent, {
      width: '500px'
    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.appointmentParams.date = formatDate(result.date);
          this.appointmentParams.startTime = formatTime(result.startTime);
          this.currentPage = 0;
          this.appointmentParams.skip = 0;
          this.fetchAppointments()
          this.loadAppointments();
        }
      }
    })
  }

  onAppointmentClicked(row: any)
  {
    this.router.navigate(['/appointments', row.id]);
  }

  updateAppointment(appointmentId : string): void {
    this.router.navigate(['/appointments/update/', appointmentId]);
  }
}
