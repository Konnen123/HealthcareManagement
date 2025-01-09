import {Component, OnInit} from '@angular/core';
import { AppointmentService } from '../../../services/appointment/appointment.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import {AppointmentFormComponent} from '../appointment-form/appointment-form.component';
import {LanguageService} from '../../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-appointment-create',
  templateUrl: './appointment-create.component.html',
  imports: [
    AppointmentFormComponent,
    TranslatePipe
  ]
})
export class AppointmentCreateComponent implements OnInit{
  constructor(
    readonly appointmentService: AppointmentService,
    readonly snackBar: MatSnackBar,
    readonly router: Router,
    readonly languageService: LanguageService
  ) {}

  ngOnInit(): void
  {
    this.languageService.setLanguage();
  }

  onFormSubmit(formData: any): void {
    console.log('Form data:', formData);
    this.appointmentService.createAsync(formData).then(appointmentId => {
      this.router.navigate([`/appointments/${appointmentId}`]);
    }).catch(() => {
      this.snackBar.open('Failed to create appointment. Please try again.', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    });
  }
}
