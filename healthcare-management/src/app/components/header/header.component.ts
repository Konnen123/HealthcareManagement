import { Component } from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {MatButtonModule} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {MatTooltip} from '@angular/material/tooltip';
import {LanguagePickerComponent} from '../language-picker/language-picker.component';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  imports: [
    MatToolbar,
    MatButtonModule,
    MatIcon,
    MatTooltip,
    TranslatePipe,
    LanguagePickerComponent
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent
{
  constructor(private readonly router: Router,
              private readonly authenticationService: AuthenticationService,) {}

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

  redirectToSymptomChecker() {
    this.router.navigate(['/symptom-checker']);
  }

  onLogoClicked() {
    this.router.navigate(['/']);
  }
}
