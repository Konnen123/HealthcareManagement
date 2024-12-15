import { Component } from '@angular/core';
import {MatCard} from '@angular/material/card';
import {RoleService} from '../../services/role/role.service';
import {NgIf} from '@angular/common';
import {MatButtonModule} from '@angular/material/button';
import {Router} from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [
    MatCard,
    NgIf,
    MatButtonModule
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent
{
  constructor(private roleService: RoleService,
              private router: Router) {}

  isUserDoctor(): boolean
  {
    return this.roleService.isUserDoctor();
  }

  isUserPatient(): boolean
  {
    return this.roleService.isUserPatient();
  }

  onViewAppointments() {
    this.router.navigate(['/appointments']);
  }

  onCreateAppointments() {
    this.router.navigate(['/appointments/create']);
  }
}
