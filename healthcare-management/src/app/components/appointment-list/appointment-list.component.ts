import {Component, OnInit} from '@angular/core';
import {AppointmentService} from '../../services/appointment/appointment.service';
import {Appointment} from '../../models/appointment.model';
import {NgForOf, NgIf, SlicePipe} from '@angular/common';
import {MatSnackBar} from '@angular/material/snack-bar';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
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
    NgIf,
    MatHeaderRowDef,
    MatRow,
    MatRowDef,
    MatTooltip,
    SlicePipe,
    MatPaginator
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

  constructor(
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar
    ) {
  }

  ngOnInit(): void {
    this.fetchAppointments();
    this.initializePage();
  }

  initializePage(): void {
    this.loadAppointments(this.pageSize, this.currentPage * this.pageSize);

  }

  loadAppointments(top: number, skip: number): void {
    this.appointmentService.getAppointmentsPaginatedAsync(top, skip).then(appointments => {
      this.appointments = appointments;
      console.log('Appointments:', this.appointments);
    }).catch(error => {
      console.error('Error while fetching appointments:', error);
      this.showErrorSnackbar('Error while fetching appointments');
    });
  }

  handlePageEvent(event: PageEvent): void {
    console.log('Page Index:', event.pageIndex);
    console.log('Page Size:', event.pageSize);
    console.log('Total Items:', this.totalCountAppointments);

    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex; // o based index , prima pagina e 10*0, a doua e 10*1
    const skip = this.currentPage * this.pageSize;
    this.loadAppointments(this.pageSize, skip);
  }

  fetchAppointments(): void{
    this.appointmentService.getAllAsync().then(app => {
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

}
