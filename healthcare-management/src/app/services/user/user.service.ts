import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {UserClient} from '../../clients/user/user.client';
import {isPlatformBrowser} from '@angular/common';
import {firstValueFrom} from 'rxjs';
import {DoctorDto} from '../../shared/dtos/doctor.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly isBrowser: boolean
  constructor(private readonly userClient: UserClient,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  public async getAllDoctorsAsync(): Promise<DoctorDto[]>
  {
    if(!this.isBrowser)
      return new Promise<any>((resolve, reject) => {});

    return await firstValueFrom(this.userClient.getAllDoctors());
  }
}
