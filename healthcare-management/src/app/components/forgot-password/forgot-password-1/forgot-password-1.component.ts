import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {FormsModule} from '@angular/forms';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {Router} from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  imports: [
    MatCard,
    FormsModule,
    MatFormField,
    MatInput,
    MatButton,
    MatLabel
  ],
  templateUrl: './forgot-password-1.component.html',
  styleUrl: './forgot-password-1.component.scss'
})
export class ForgotPassword1Component {
  email: string = '';

  constructor(
    readonly router: Router
  ) { }

  redirectToLogin() : void {
    this.router.navigate(['/login']);
  }

  redirectToForgotPassword() : void {
    this.router.navigate(['/forgot-password/change-password']);
  }
}
