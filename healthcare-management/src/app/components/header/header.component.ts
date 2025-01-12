import { Component, OnInit } from '@angular/core';
import { MatToolbar } from '@angular/material/toolbar';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatTooltip } from '@angular/material/tooltip';
import { LanguagePickerComponent } from '../language-picker/language-picker.component';
import { TranslatePipe } from '@ngx-translate/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [
    MatToolbar,
    MatButtonModule,
    MatIcon,
    MatTooltip,
    TranslatePipe,
    LanguagePickerComponent,
    NgIf
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  userRole: string | null = null;

  constructor(
    private readonly router: Router,
    private readonly authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    this.userRole = this.authenticationService.getUserRole();
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

  redirectToAppointments() {
    this.router.navigate(['/appointments']);
  }

  redirectToSymptomChecker() {
    this.router.navigate(['/symptom-checker']);
  }

  redirectToCreateAppointment() {
    this.router.navigate(['/appointments/create']);
  }

  redirectToHomepage() {
    this.router.navigate(['/home']);
  }

  onLogoClicked() {
    this.router.navigate(['/']);
  }
}
