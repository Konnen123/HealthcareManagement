import { Component } from '@angular/core';
import { MatCard } from '@angular/material/card';
import {MatError, MatFormField, MatLabel} from '@angular/material/form-field';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { MatSelect, MatOption } from '@angular/material/select';
import {AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {Router} from '@angular/router';
import { CustomValidators } from '../../shared/custom-validators';
import {AuthenticationService} from '../../services/authentication/authentication.service';

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
    CommonModule,
    MatError
  ],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  signupForm: FormGroup;

  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  firstName: string = '';
  lastName: string = '';
  phoneNumber: string = '';
  dateOfBirth: string = '';
  role: string = '';

  constructor(
    readonly fb: FormBuilder,
    readonly authenticationService: AuthenticationService,
    readonly router: Router
  ) {
    this.signupForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      dateOfBirth: ['', [Validators.required, CustomValidators.dateOfBirthValidator]],
      email: ['', [Validators.required, Validators.email]],
      role: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required, CustomValidators.passwordsMatch('password')]]
    });
    this.signupForm.addValidators(this.passwordMatchValidator);
  }

  passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    return password === confirmPassword ? null : { mismatch: true };
  }


  


  onSubmit(): void {
    const formData = { ... this.signupForm.value };
    if (this.signupForm.valid) {
      this.authenticationService.registerAsync(formData).then((response) => {
        console.log('Register successful :', response);
        this.router.navigate(['/login']);
      }).catch((error) => {
        console.error('Error at register ', error);
      });
    }
  }

}
