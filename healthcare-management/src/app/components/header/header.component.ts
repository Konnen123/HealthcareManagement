import { Component } from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {MatButtonModule} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {MatTooltip} from '@angular/material/tooltip';

@Component({
  selector: 'app-header',
  imports: [
    MatToolbar,
    MatButtonModule,
    MatIcon,
    MatTooltip
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(private readonly router: Router,
              private readonly authenticationService: AuthenticationService) {}

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

  redirectToSymptomChecker() {
    this.router.navigate(['/symptom-checker']);
  }
}
