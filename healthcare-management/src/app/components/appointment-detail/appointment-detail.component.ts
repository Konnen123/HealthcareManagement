import {Component, OnInit} from '@angular/core';
import {
  MatCard
} from '@angular/material/card';
import {MatIcon} from '@angular/material/icon';
import {ActivatedRoute, Params} from '@angular/router';
import {AppointmentService} from '../../services/appointment/appointment.service';
import {Appointment} from '../../models/appointment.model';
import {HttpErrorResponse} from '@angular/common/http';
import {NgIf} from '@angular/common';
import {MatSnackBar} from '@angular/material/snack-bar';
import {AbstractControl, ValidationErrors} from '@angular/forms';

@Component({
  selector: 'app-appointment-detail',
  imports: [
    MatCard,
    MatIcon,
    NgIf,
  ],
  templateUrl: './appointment-detail.component.html',
  styleUrl: './appointment-detail.component.scss'
})
export class AppointmentDetailComponent implements OnInit{
  appointmentId!: string;
  appointmentDetails!: Appointment;
  errorMessage: string | null = null;
  loading: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar
  ) {
  }

  ngOnInit() {
    this.route.params.subscribe(async (params:Params) => {
      this.appointmentId = params['id'];
      this.fetchAppointmentDetails();
    });
  }


  fetchAppointmentDetails(): void {
    this.loading = true;
    this.errorMessage = null; // Reset error state
    this.appointmentService.getByIdAsync(this.appointmentId)
      .then((appointment) => {
        this.appointmentDetails = appointment;
        this.loading = false;
      })
      .catch((error: HttpErrorResponse) => {
        this.loading = false;
        this.errorMessage = error.status === 404
          ? `Appointment not found.`
          : 'An unexpected error occurred.';
        this.showErrorSnackbar();
      });
  }

  showErrorSnackbar(): void {
    this.snackBar.open(this.errorMessage || 'An error occurred.', 'Close', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
    });
  }

  isValidGuid(control: AbstractControl): ValidationErrors | null {
    const guidRegex =
      /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(control.value) ? null : { invalidGuid: true };
  }
}
