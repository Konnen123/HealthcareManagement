import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {UserClient} from '../../clients/user.client';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private readonly isBrowser!: boolean;
  constructor(private userClient : UserClient, @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId)
  }

  public async registerAsync(){
    try {
      const result = await firstValueFrom(this.userClient.register());
      console.log('Server response in the service :', result);
      return result;
    } catch (error){
      console.error('Error while registering in service', error);
      throw error;
    }

  }
}
