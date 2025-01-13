import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {firstValueFrom} from 'rxjs';
import {MailClient} from '../../clients/mail/mail.client';

@Injectable({
  providedIn: 'root'
})
export class MailService {

  private readonly isBrowser: boolean
  constructor(private readonly mailClient: MailClient,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  public async sendForgotPasswordEmailAsync(email: string): Promise<any>
  {
    return await firstValueFrom(this.mailClient.sendForgotPasswordEmail(email));
  }
}
