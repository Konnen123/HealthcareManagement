import {Component, OnInit} from '@angular/core';
import {MatCard} from '@angular/material/card';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatError, MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {Router} from '@angular/router';
import {NgIf} from '@angular/common';
import {MatSnackBar} from '@angular/material/snack-bar';
import {MailService} from '../../../services/mail/mail.service';
import {TranslatePipe} from '@ngx-translate/core';
import {LanguageService} from '../../../services/language/language.service';

@Component({
  selector: 'app-forgot-password',
  imports: [
    MatCard,
    FormsModule,
    MatFormField,
    MatInput,
    MatButton,
    MatLabel,
    ReactiveFormsModule,
    MatError,
    NgIf,
    TranslatePipe
  ],
  templateUrl: './forgot-password-1.component.html',
  styleUrl: './forgot-password-1.component.scss'
})
export class ForgotPassword1Component{
  forgotPasswordForm!: FormGroup;

  constructor(
    readonly router: Router,
    readonly mailService: MailService,
    readonly snackBar: MatSnackBar,
    private fb: FormBuilder,
    private readonly languageService: LanguageService
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
    this.languageService.setLanguage();
  }

  redirectToLogin() : void {
    this.router.navigate(['/login']);
  }
  onSubmit() : void {
    if (this.forgotPasswordForm.valid) {
      const email = this.forgotPasswordForm.value.email;
      this.mailService.sendForgotPasswordEmailAsync(this.forgotPasswordForm.value.email).then(() => {
        this.snackBar.open('Please check your email', 'Close', {
          duration: 5000,
        });
      }).catch((error) => {
        this.snackBar.open(error.description + '. Please Try again.', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar'],
        });
      });
    }
  }
}
