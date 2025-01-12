import {Component, OnInit} from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatError, MatFormField, MatLabel} from '@angular/material/form-field';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {MatInput} from '@angular/material/input';
import {NgIf} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {ActivatedRoute, Router} from '@angular/router';
import {CustomValidators} from '../../../shared/custom-validators';
import {AuthenticationService} from '../../../services/authentication/authentication.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {LanguageService} from '../../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';


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
    MatError,
    TranslatePipe
  ],
  templateUrl: './forgot-password-2.component.html',
  styleUrl: './forgot-password-2.component.scss'
})
export class ForgotPassword2Component implements OnInit {
  resetPasswordForm: FormGroup;
  token: string = '';

  constructor(
    readonly fb: FormBuilder,
    readonly router: Router,
    readonly route: ActivatedRoute,
    readonly authService: AuthenticationService,
    readonly snackBar: MatSnackBar,
    private readonly languageService: LanguageService
    ) {
    this.languageService.setLanguage();
    this.resetPasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required]]
    });

    this.resetPasswordForm.get('confirmPassword')?.setValidators([
      Validators.required,
      this.passwordsMatchValidator.bind(this)
    ]);

    this.resetPasswordForm.get('newPassword')?.valueChanges.subscribe(() => {
      this.resetPasswordForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  ngOnInit(): void{

    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
  }

  passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
    const newPassword = this.resetPasswordForm?.get('newPassword')?.value;
    const confirmPassword = control.value;

    return newPassword && confirmPassword && newPassword !== confirmPassword
      ? { passwordsMismatch: true }
      : null;
  }

  onSubmit(): void {
    if (this.resetPasswordForm.valid) {
      const payload = {
        password: this.resetPasswordForm.get('newPassword')?.value,
        token: decodeURIComponent(this.token).replace(/ /g, '+')
      };

      this.authService.resetPasswordAsync(payload)
        .then(() => {
          this.router.navigate(['/login']);
          this.snackBar.open('Password reset successfully', 'Close', {
            panelClass: ['error-snackbar'],
            duration: 3000
          });
        })
        .catch((error) => {
          console.error('Error while resetting password', error);
        });
    }
  }
}
