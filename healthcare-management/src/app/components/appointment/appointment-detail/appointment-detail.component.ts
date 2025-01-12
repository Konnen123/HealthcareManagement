import {Component, OnInit} from '@angular/core';
import {
  MatCard
} from '@angular/material/card';
import {MatIcon} from '@angular/material/icon';
import {ActivatedRoute, Params, Router} from '@angular/router';
import {HttpErrorResponse} from '@angular/common/http';
import {NgIf} from '@angular/common';
import {MatSnackBar} from '@angular/material/snack-bar';
import {AbstractControl, ValidationErrors} from '@angular/forms';
import {MatButton} from '@angular/material/button';
import {Appointment} from '../../../models/appointment.model';
import {AppointmentService} from '../../../services/appointment/appointment.service';
import {TranslatePipe} from '@ngx-translate/core';
import {LanguageService} from '../../../services/language/language.service';
import {RoleService} from '../../../services/role/role.service';

@Component({
  selector: 'app-appointment-detail',
  imports: [
    MatCard,
    MatIcon,
    NgIf,
    MatButton,
    TranslatePipe
  ],
  templateUrl: './appointment-detail.component.html',
  styleUrl: './appointment-detail.component.scss'
})
export class AppointmentDetailComponent implements OnInit{
  appointmentId!: string;
  appointmentDetails!: Appointment;
  loading: boolean = true;

  constructor(
    readonly route: ActivatedRoute,
    readonly appointmentService: AppointmentService,
    readonly snackBar: MatSnackBar,
    readonly router: Router,
    readonly languageService: LanguageService,
    readonly roleService: RoleService
  ) {
  }

  ngOnInit() {
    this.languageService.setLanguage();

    this.route.params.subscribe(async (params:Params) => {
      this.appointmentId = params['id'];
      this.fetchAppointmentDetails();
    });
  }

  onDelete() {
    this.appointmentService.deleteAsync(this.appointmentId).then(() => {
      this.router.navigate(['/appointments']).then(r => console.log('Navigated to appointments.'));
    }).catch((error) => {
      this.showErrorSnackbar('An error occurred while deleting the appointment.');
    });
  }

  fetchAppointmentDetails(): void {
    this.loading = true;
    this.appointmentService.getByIdAsync(this.appointmentId)
      .then((appointment) => {
        this.appointmentDetails = appointment;
        this.loading = false;
      })
      .catch((error: HttpErrorResponse) => {
        this.loading = false;
        this.showErrorSnackbar(error.status === 404
          ? `Appointment not found.`
          : 'An unexpected error occurred.');
      });
  }

  private showErrorSnackbar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['error-snackbar'],
    });
  }

  isValidGuid(control: AbstractControl): ValidationErrors | null {
    const guidRegex =
      /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(control.value) ? null : { invalidGuid: true };
  }

  onAppointmentsClick()
  {
    this.router.navigate(['/appointments']);
  }

  onUpdate()
  {
    this.router.navigate([`/appointments/update/${this.appointmentId}`]);
  }
}
