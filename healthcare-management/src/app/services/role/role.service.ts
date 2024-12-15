import { Injectable } from '@angular/core';
import {AuthenticationService} from '../authentication/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private authenticationService: AuthenticationService) { }

  public isUserDoctor(): boolean
  {
    return this.authenticationService.getUserRole() === 'DOCTOR';
  }

  public isUserPatient(): boolean
  {
    return this.authenticationService.getUserRole() === 'PATIENT';
  }
}
