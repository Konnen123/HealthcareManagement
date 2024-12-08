import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

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

  onSubmit(): void {
    console.log('Email:', this.email);
    console.log('Password:', this.password);
  }
}
