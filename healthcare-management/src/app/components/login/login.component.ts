import {Component, OnInit} from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatError, MatFormField, MatLabel} from '@angular/material/form-field';
import {MatButton} from '@angular/material/button';
import {MatInput} from '@angular/material/input';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {NgIf} from '@angular/common';
import {LanguageService} from '../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';


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
    NgIf,
    TranslatePipe
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginForm!: FormGroup;

  constructor(
    readonly router: Router,
    readonly authenticationService: AuthenticationService,
    readonly fb: FormBuilder,
    readonly snackBar: MatSnackBar,
    private readonly languageService: LanguageService
  ) {}

  ngOnInit(): void
  {
    this.languageService.setLanguage();
    this.loginForm = this.fb.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    const requestData = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };

    this.authenticationService.loginAsync(requestData).then((response) => {
      const accessToken = response.accessToken;
      const refreshToken = response.refreshToken;

      this.authenticationService.setCookie('token', accessToken);
      this.authenticationService.setCookie('refreshToken', refreshToken);
      this.router.navigate(['/home']);
    }).catch((error) => {
      console.error('Error from the service:', error);
      this.snackBar.open(error.error.description, 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    });
  }

  redirectToSignup(): void {
    this.router.navigate(['/signup']);
  }

  redirectToForgotPassword(): void {
    this.router.navigate(['/forgot-password']);
  }
}
