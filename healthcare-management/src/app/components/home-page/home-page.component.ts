import {Component, OnInit} from '@angular/core';
import {MatCard} from '@angular/material/card';
import {RoleService} from '../../services/role/role.service';
import {NgIf} from '@angular/common';
import {MatButtonModule} from '@angular/material/button';
import {Router} from '@angular/router';
import {LanguageService} from '../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-home-page',
  imports: [
    MatCard,
    NgIf,
    MatButtonModule,
    TranslatePipe
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit
{
  constructor(private readonly roleService: RoleService,
              private readonly router: Router,
              private readonly languageService: LanguageService) {}

  ngOnInit(): void
  {
    this.languageService.setLanguage();
  }

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
