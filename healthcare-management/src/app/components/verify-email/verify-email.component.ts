import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-verify-email',
  imports: [
    MatCardActions,
    MatCardContent,
    MatCardTitle,
    MatButton,
    MatCard,
    MatCardHeader
  ],
  templateUrl: './verify-email.component.html',
  styleUrl: './verify-email.component.scss'
})
export class VerifyEmailComponent implements OnInit {
  status: string = '';
  description: string = '';
  message: string = '';
  isSuccess: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.status = params['status'] || '';
      this.description = params['description'] || '';

      this.generateMessage();
    });
  }

  generateMessage(): void {
    if (this.status === 'success') {
      this.isSuccess = true;
      this.message = 'Your email has been successfully verified! 🎉';
    } else if (this.status === 'error') {
      this.isSuccess = false;
      switch (this.description) {
        case 'verification_failed':
          this.message = 'The email verification failed. Please try again. 💔';
          break;
        case 'token_expired':
          this.message = 'The verification token has expired. Please request a new verification email. ⌛';
          break;
        case 'token_not_found':
          this.message = 'The verification token is invalid or missing. Please check your email link. 🚫';
          break;
        default:
          this.message = 'An unknown error occurred during email verification. Please contact support. 🛠️';
      }
    } else {
      this.message = 'Invalid request. Please check the link or contact support. ❓';
    }
  }

  goToHomepage(): void {
    this.router.navigate(['/']);
  }
}
