import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FilterDialogComponent } from './filter-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogRef } from '@angular/material/dialog';
import { LanguageService } from '../../services/language/language.service';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { EventEmitter } from '@angular/core';

describe('FilterDialogComponent', () => {
  let component: FilterDialogComponent;
  let fixture: ComponentFixture<FilterDialogComponent>;
  let mockDialogRef: jasmine.SpyObj<MatDialogRef<FilterDialogComponent>>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage']);

    const translateServiceMock = {
      instant: jasmine.createSpy('instant').and.returnValue(''),
      get: jasmine.createSpy('get').and.returnValue(of('')),
      currentLang: 'en',
      onLangChange: new EventEmitter(),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter(),
    };

    await TestBed.configureTestingModule({
      imports: [
        FilterDialogComponent,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatButtonModule,
        MatIconModule,
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: MatDialogRef, useValue: mockDialogRef },
        { provide: LanguageService, useValue: mockLanguageService },
        { provide: TranslateService, useValue: translateServiceMock },
        provideNativeDateAdapter(), // Fix for the missing DateAdapter
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(FilterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty fields', () => {
    expect(component.filterForm.value).toEqual({
      startTime: '',
      date: '',
    });
  });

  it('should call languageService.setLanguage on init', () => {
    expect(mockLanguageService.setLanguage).toHaveBeenCalled();
  });

  it('should close the dialog with form values when applyFilters is called', () => {
    const mockFormValues = { startTime: '10:00', date: '2024-01-01' };
    component.filterForm.setValue(mockFormValues);

    component.applyFilters();

    expect(mockDialogRef.close).toHaveBeenCalledWith(mockFormValues);
  });

});
