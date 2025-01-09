import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatError, MatFormField, MatLabel} from '@angular/material/form-field';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    MatCard,
    MatFormField,
    MatButton,
    MatInput,
    MatLabel,
    MatError,
    FormsModule,
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    readonly router: Router,
    readonly authenticationService: AuthenticationService,
    readonly fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    const requestData ={
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };

    this.authenticationService.loginAsync(requestData).then((response) => {
      const accessToken = response.accessToken;
      const refreshToken = response.refreshToken;

      this.authenticationService.setCookie('token', accessToken);
      this.authenticationService.setCookie('refreshToken', refreshToken);
      console.log('Response from the service:', response);
      this.router.navigate(['/']);
    }).catch((error) => {
      console.error('Error from the service:', error);
    });
  }

  redirectToSignup(): void {
    this.router.navigate(['/signup']);
  }
}
