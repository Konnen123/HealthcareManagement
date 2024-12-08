import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {UserClient} from '../../clients/user.client';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private readonly isBrowser!: boolean;
  constructor(readonly userClient : UserClient, @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId)
  }

}
