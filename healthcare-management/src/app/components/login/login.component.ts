import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-login',
  imports: [
    MatCard,
    MatFormField,
    MatButton,
    MatInput,
    MatLabel,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(
    readonly router: Router,
    readonly authenticationService: AuthenticationService
  ) { }

  onSubmit(): void {
    const requestData ={
      email: this.email,
      password: this.password
    };

    this.authenticationService.loginAsync(requestData).then((response) => {
      const accessToken = response.accessToken;
      this.authenticationService.setCookie('token', accessToken);
      console.log('Response from the service:', response);
      this.router.navigate(['appointments']);
    }).catch((error) => {
      console.error('Error from the service:', error);
    });
  }

  redirectToSignup(): void {
    this.router.navigate(['/signup']);
  }
}
