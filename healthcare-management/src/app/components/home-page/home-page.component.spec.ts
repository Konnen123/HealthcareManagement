import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomePageComponent } from './home-page.component';
import {EventEmitter} from '@angular/core';
import {of} from 'rxjs';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {LanguageService} from '../../services/language/language.service';
import {RoleService} from '../../services/role/role.service';

describe('HomePageComponent', () => {
  let component: HomePageComponent;
  let fixture: ComponentFixture<HomePageComponent>;

  beforeEach(async () => {
    const roleServiceSpy = jasmine.createSpyObj('RoleService', ['isUserDoctor', 'isUserPatient']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage']);
    const translateService = jasmine.createSpyObj('TranslateService', ['instant', 'get']);
    const translateServiceMock = {
      currentLang: 'ro',
      onLangChange: new EventEmitter<LangChangeEvent>(),
      use: translateService.get,
      get: translateService.get.and.returnValue(of('')),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter()
    }
    await TestBed.configureTestingModule({
      imports: [HomePageComponent],
      providers: [
        { provide: RoleService, useValue: roleServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
