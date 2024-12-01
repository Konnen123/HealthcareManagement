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
    NgForOf,
    MatProgressSpinner,
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
  pageSize: number = 10;
  currentPage: number = 0;
  loading: boolean = true;
  displayedColumns: string[] = ['patientId', 'doctorId', 'date', 'time'];

  constructor(
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar
    ) {}

  ngOnInit(): void {
    this.loadAppointments(this.pageSize, this.currentPage * this.pageSize);
  }

  loadAppointments(top: number, skip: number): void {
    this.appointmentService.getAppointmentsPaginatedAsync(top, skip).then(appointments => {
      this.appointments = appointments;
      this.loading = false;
    }).catch(error => {
      console.error('Error while fetching appointments:', error);
      this.showErrorSnackbar('Error while fetching appointments');
    });
  }


  onPageChange(event: PageEvent): void {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    const skip = this.currentPage * this.pageSize;
    this.loadAppointments(this.pageSize, skip);
  }

  // fetchAppointments(): void{
  //   this.loading = true;
  //   this.appointmentService.getAllAsync().then(appointments => {
  //     this.appointments = appointments;
  //     this.loading = false;
  //   }).catch(error => {
  //     console.error('Error while fetching appointments:', error);
  //     this.loading = false;
  //     this.showErrorSnackbar('Error while fetching appointments');
  //   })
  // }

  private showErrorSnackbar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['error-snackbar'],
    });
  }

}
