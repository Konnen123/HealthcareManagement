import { Component } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { MatSelect, MatOption } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService} from '../../services/users/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-signup',
  imports: [
    MatCard,
    MatFormField,
    MatButton,
    MatInput,
    MatLabel,
    MatSelect,
    MatOption,
    FormsModule,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  firstName: string = '';
  lastName: string = '';
  phoneNumber: string = '';
  dateOfBirth: string = '';
  role: string = '';

  constructor(
    readonly authService: AuthService,
    readonly router: Router
  ) {
  }

  onSubmit(): void {
    const requestData = {
      email: this.email,
      password: this.password,
      firstName: this.firstName,
      lastName: this.lastName,
      phoneNumber: this.phoneNumber,
      dateOfBirth: this.dateOfBirth,
      role: this.role.toUpperCase()
    };
    this.authService.registerAsync(requestData).then((response) => {
        console.log('Register successful :', response);
        this.router.navigate(['login']);
    }).catch((error) => {
        console.error('Error at register ', error);
    });
  }

  isFormValid(): boolean {
    return (
      !!this.firstName &&
      !!this.lastName &&
      !!this.phoneNumber &&
      !!this.dateOfBirth &&
      !!this.role &&
      !!this.email &&
      !!this.password &&
      this.password === this.confirmPassword
    );
  }
}
