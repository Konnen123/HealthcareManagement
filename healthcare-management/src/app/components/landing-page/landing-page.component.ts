import { Component } from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {MatCard} from '@angular/material/card';
import {MatButton} from '@angular/material/button';
import {RouterLink} from '@angular/router';
import {MatIcon} from '@angular/material/icon';
import {NgForOf} from '@angular/common';
import {LanguageService} from '../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';

interface onInit {
}

@Component({
  selector: 'app-landing-page',
  imports: [
    MatToolbar,
    MatCard,
    MatButton,
    RouterLink,
    MatIcon,
    NgForOf,
    TranslatePipe
  ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent implements onInit{

  features = [
    { icon: 'assignment', title: 'features.MANAGE_RECORDS.TITLE', description: 'features.MANAGE_RECORDS.DESCRIPTION' },
    { icon: 'healing', title: 'features.PREDICT_HEALTH_RISKS.TITLE', description: 'features.PREDICT_HEALTH_RISKS.DESCRIPTION' },
    { icon: 'lock', title: 'features.SECURE_ACCESS.TITLE', description: 'features.SECURE_ACCESS.DESCRIPTION' },
  ];

  constructor(
    private readonly languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.languageService.setLanguage();
  }
}
