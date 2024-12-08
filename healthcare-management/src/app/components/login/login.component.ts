import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {AuthService} from '../../services/users/auth.service';
import {Router} from '@angular/router';

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
    readonly authService: AuthService,
    readonly router: Router
  ) { }

  onSubmit(): void {
    const requestData ={
      email: this.email,
      password: this.password
    };

    this.authService.loginAsync(requestData).then((response) => {
      console.log('Response from the service:', response);
      this.router.navigate(['appointments']);
    }).catch((error) => {
      console.error('Error from the service:', error);
    });
  }
}
