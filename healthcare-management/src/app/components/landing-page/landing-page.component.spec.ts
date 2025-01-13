import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LandingPageComponent } from './landing-page.component';
import { LanguageService } from '../../services/language/language.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { of } from 'rxjs';
import { NgForOf } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { EventEmitter } from '@angular/core';

describe('LandingPageComponent', () => {
  let component: LandingPageComponent;
  let fixture: ComponentFixture<LandingPageComponent>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'ro',
      onLangChange: new EventEmitter(),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter(),
    };

    await TestBed.configureTestingModule({
      imports: [
        MatToolbarModule,
        MatCardModule,
        MatButtonModule,
        MatIconModule,
        RouterModule.forRoot([]),
        NgForOf,
        LandingPageComponent,
        TranslatePipe
      ],
      providers: [
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LandingPageComponent);
    component = fixture.componentInstance;
    mockLanguageService = TestBed.inject(LanguageService) as jasmine.SpyObj<LanguageService>;

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize features correctly', () => {
    expect(component.features.length).toBe(3);
    expect(component.features[0].icon).toBe('assignment');
    expect(component.features[1].icon).toBe('healing');
    expect(component.features[2].icon).toBe('lock');
  });

  it('should call setLanguage on initialization', () => {
    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
  });
});
