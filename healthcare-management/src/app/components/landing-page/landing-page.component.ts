import { Component } from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {MatCard} from '@angular/material/card';
import {MatButton} from '@angular/material/button';
import {RouterLink} from '@angular/router';
import {MatIcon} from '@angular/material/icon';
import {NgForOf} from '@angular/common';

@Component({
  selector: 'app-landing-page',
  imports: [
    MatToolbar,
    MatCard,
    MatButton,
    RouterLink,
    MatIcon,
    NgForOf
  ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent {
  features = [
    { icon: 'assignment', title: 'Manage Records', description: 'Efficiently handle patient data and appointments.' },
    { icon: 'healing', title: 'Predict Health Risks', description: 'Use AI to predict potential health issues.' },
    { icon: 'lock', title: 'Secure Access', description: 'Ensure data privacy with robust authentication.' },
  ];
}
