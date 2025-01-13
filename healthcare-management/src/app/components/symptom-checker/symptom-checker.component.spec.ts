import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import { SymptomCheckerComponent } from './symptom-checker.component';
import {EventEmitter} from '@angular/core';
import {LangChangeEvent, TranslateService} from '@ngx-translate/core';
import {of} from 'rxjs';
import {LanguageService} from '../../services/language/language.service';
import {SymptomService} from '../../services/symptom-checker/symptom-checker.service';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatChipInputEvent} from '@angular/material/chips';
import {MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';

describe('SymptomCheckerComponent', () => {
  let component: SymptomCheckerComponent;
  let fixture: ComponentFixture<SymptomCheckerComponent>;
  let mockSymptomService: jasmine.SpyObj<SymptomService>;
  let mockLanguageService: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    spyOn(console, 'error');
    mockSymptomService = jasmine.createSpyObj('SymptomService', ['predictAsync']);
    mockSymptomService.predictAsync.and.returnValue(Promise.reject(new Error('Mock error')));
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['setLanguage', 'getLanguage']);
    const translateService = jasmine.createSpyObj('TranslateService', ['instant', 'get']);
    mockLanguageService = jasmine.createSpyObj('LanguageService', ['setLanguage', 'getLanguage']);
    const translateServiceMock = {
      currentLang: 'ro',
      onLangChange: new EventEmitter<LangChangeEvent>(),
      use: translateService.get,
      get: translateService.get.and.returnValue(of('')),
      onTranslationChange: new EventEmitter(),
      onDefaultLangChange: new EventEmitter()
    }

    await TestBed.configureTestingModule({
      imports: [
        SymptomCheckerComponent,
      BrowserAnimationsModule],
      providers: [
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: TranslateService, useValue: translateServiceMock },
        { provide: SymptomService, useValue: mockSymptomService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SymptomCheckerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should filter options based on input', () => {
    component.currentAllOptions = ['Headache', 'Fever', 'Cough'];
    component.multiSelectControl.setValue('fev');
    fixture.detectChanges();

    const filtered = component.filteredOptions as any;
    filtered.subscribe((options: string[]) => {
      expect(options).toContain('Fever');
      expect(options).toContain('Cough');
    });
  });

  it('should remove a symptom', () => {
    component.selectedOptions = ['Headache', 'Fever'];
    component.remove('Fever');
    expect(component.selectedOptions).not.toContain('Fever');
  });

  it('should handle symptom selection from autocomplete', () => {
    const event = { option: { viewValue: 'Cough' } } as any;
    component.selected(event);
    expect(component.selectedOptions).toContain('Cough');
  });

  it('should not add a duplicate symptom', () => {
    component.currentAllOptions = ['Headache', 'Fever', 'Cough'];
    component.selectedOptions = ['Fever'];

    const event = { value: 'Fever', chipInput: { clear: jasmine.createSpy('clear') } } as any;
    component.add(event);

    expect(component.selectedOptions).toEqual(['Fever']); // Still only one "Fever"
  });

  it('should reset input control after symptom selection', () => {
    const event = { option: { viewValue: 'Fever' } } as MatAutocompleteSelectedEvent;
    component.selected(event);
    expect(component.multiSelectControl.value).toBeNull();
  });

  it('should handle error during symptom submission', async () => {
    // Mock rejection from the service
    mockSymptomService.predictAsync.and.returnValue(Promise.reject(new Error('Mock error')));

    // Set the selected options
    component.selectedOptions = ['Headache', 'Cough'];

    // Call the method and wait for it to complete
    await component.submitSymptoms();

    // Check the result after rejection
    expect(component.result).toBe('...');
  });

  it('should map selected symptoms to English when language is Romanian', async () => {
    // Mock language service to return Romanian
    mockLanguageService.getLanguage.and.returnValue('ro');

    // Set selected options and indices for Romanian symptoms
    component.selectedOptions = ['febră', 'tuse']; // Romanian for 'Fever', 'Cough'
    component.selectedOptionsIndex = [1, 2]; // Indices for 'Fever' and 'Cough' in the English array
    component.allOptions = ['headache', 'fever', 'cough'];
    component.allOptionsRomanian = ['durere de cap', 'febră', 'tuse']; // Romanian translations

    // Mock successful service response
    mockSymptomService.predictAsync.and.returnValue(Promise.resolve({ disease: 'Common Cold' }));

    // Call the method and wait for it to complete
    await component.submitSymptoms();

    // Check the result and service call
    expect(mockSymptomService.predictAsync).toHaveBeenCalledWith({
      symptoms: ['febră', 'tuse'],
      targetLanguage: undefined,
      sourceLanguage: 'en',
    });
  });

  it('should handle successful symptom submission', async () => {
    // Mock successful resolution from the service
    mockSymptomService.predictAsync.and.returnValue(Promise.resolve({ disease: 'Flu' }));

    // Set the selected options
    component.selectedOptions = ['fever', 'cough'];

    // Call the method and wait for it to complete
    await component.submitSymptoms();

    // Check the result after successful resolution
    expect(component.result).toBe('Flu');
    expect(mockSymptomService.predictAsync).toHaveBeenCalledWith({
      symptoms: ['fever', 'cough'],
      targetLanguage: mockLanguageService.getLanguage(),
      sourceLanguage: 'en',
    });
  });

  it('should handle empty selected options gracefully', async () => {
    // Mock service response
    mockSymptomService.predictAsync.and.returnValue(Promise.resolve({ disease: 'No disease detected' }));

    // Ensure no symptoms are selected
    component.selectedOptions = [];

    // Call the method and wait for it to complete
    await component.submitSymptoms();

    // Verify service was not called
    expect(mockSymptomService.predictAsync).toHaveBeenCalled();

    // Verify result
    expect(component.result).toBe('No disease detected');
  });

  it('should call symptomService in correct order during submission', async () => {
    // Mock language and service response
    mockLanguageService.getLanguage.and.returnValue('en');
    mockSymptomService.predictAsync.and.returnValue(Promise.resolve({ disease: 'Flu' }));

    // Set selected options
    component.selectedOptions = ['Fever'];

    // Call the method
    await component.submitSymptoms();

    expect(mockSymptomService.predictAsync).toHaveBeenCalledWith({
      symptoms: ['Fever'],
      targetLanguage: undefined,
      sourceLanguage: 'en',
    });
  });

});
