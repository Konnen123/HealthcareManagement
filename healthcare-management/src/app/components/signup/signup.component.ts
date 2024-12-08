import { Component } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { MatSelect, MatOption } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService} from '../../services/users/auth.service';

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
    readonly authService: AuthService
  ) {
  }

  onSubmit(): void {
    console.log('Email:', this.email);
    console.log('Password:', this.password);
    console.log('Confirm Password:', this.confirmPassword);
    console.log('First Name:', this.firstName);
    console.log('Last Name:', this.lastName);
    console.log('Phone Number:', this.phoneNumber);
    console.log('Date of Birth:', this.dateOfBirth);
    console.log('Role:', this.role);



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
