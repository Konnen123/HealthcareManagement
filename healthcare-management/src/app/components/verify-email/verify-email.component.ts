import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {MatButton} from '@angular/material/button';
import {AuthenticationService} from '../../services/authentication/authentication.service';

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
  isSuccess: boolean = true;

  constructor(
    readonly activatedRoute: ActivatedRoute,
    readonly router: Router,
    readonly authenticationService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(async params => {
      const verifyToken = params['token'] || '';

      try{
        await this.authenticationService.verifyEmailAsync(verifyToken);
      }
      catch (error){

      }
      finally{
        this.status = 'success';
        this.isSuccess = true;
        this.message = 'Your email has been successfully verified! 🎉';
      }

    });
  }

  goToHomepage(): void {
    this.router.navigate(['/']);
  }
}
