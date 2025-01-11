import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatError, MatFormField, MatLabel} from '@angular/material/form-field';
import {AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatInput} from '@angular/material/input';
import {NgIf} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {Router} from '@angular/router';
import {CustomValidators} from '../../../shared/custom-validators';


@Component({
  selector: 'app-forgot-password-2',
  imports: [
    MatCard,
    MatFormField,
    FormsModule,
    MatInput,
    NgIf,
    MatButton,
    ReactiveFormsModule,
    MatLabel,
    MatError
  ],
  templateUrl: './forgot-password-2.component.html',
  styleUrl: './forgot-password-2.component.scss'
})
export class ForgotPassword2Component {
  resetPasswordForm: FormGroup;

  newPassword: string = '';
  confirmPassword: string = '';

  constructor(private fb: FormBuilder, private router: Router) {
    this.resetPasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required, CustomValidators.passwordsMatch('password')]]
    });
    this.resetPasswordForm.addValidators(this.passwordMatchValidator);
  }

  passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    return newPassword === confirmPassword ? null : { mismatch: true };
  }

  onSubmit(): void {
    this.router.navigate(['/login']);
  }
}
