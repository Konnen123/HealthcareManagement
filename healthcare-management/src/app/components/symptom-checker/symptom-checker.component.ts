import {Component, OnInit} from '@angular/core';
import {MatFormField} from '@angular/material/form-field';
import {
  MatChipInput,
  MatChipInputEvent,
  MatChipsModule
} from '@angular/material/chips';
import {FormControl, ReactiveFormsModule} from '@angular/forms';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { MatIconModule} from '@angular/material/icon';
import {AsyncPipe, NgForOf, NgIf} from '@angular/common';
import {
  MatAutocomplete,
  MatAutocompleteModule, MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
  MatOption
} from '@angular/material/autocomplete';
import {MatInputModule} from '@angular/material/input';
import {map, Observable, of, startWith} from 'rxjs';
import {MatButton} from '@angular/material/button';
import symptoms from '../../files/symptoms.json';
import symptomsRomanian from '../../files/symptoms.ro.json';
import {SymptomService} from '../../services/symptom-checker/symptom-checker.service';
import {LanguageService} from '../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-symptom-checker',
  imports: [
    MatFormField,
    NgForOf,
    ReactiveFormsModule,
    MatAutocompleteTrigger,
    MatChipInput,
    MatAutocomplete,
    MatOption,
    MatChipsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule,
    AsyncPipe,
    MatButton,
    NgIf,
    TranslatePipe
  ],
  templateUrl: './symptom-checker.component.html',
  styleUrl: './symptom-checker.component.scss'
})
export class SymptomCheckerComponent implements OnInit{
  separatorKeysCodes: number[] = [ENTER, COMMA];
  multiSelectControl = new FormControl('');
  currentAllOptions!: string[];
  allOptions: string[] = symptoms.symptoms;
  allOptionsRomanian: string[] = symptomsRomanian.symptoms;
  filteredOptions: Observable<string[]> = of([]);
  selectedOptions: string[] = [];
  selectedOptionsIndex: number[] = [];
  result: string = '';

  constructor(
    readonly symptomService: SymptomService,
    readonly languageService: LanguageService
  ) {}

  ngOnInit() {
    this.languageService.setLanguage();
    this.currentAllOptions = this.languageService.getLanguage() === 'ro' ? this.allOptionsRomanian : this.allOptions;
      this.filteredOptions = this.multiSelectControl.valueChanges.pipe(
        startWith(''),
        map((value: string | null) => this._filter(value))
      );
  }

  private _filter(value: string | null): string[] {
    const filterValue = (value ?? '').toLowerCase();
    return this.currentAllOptions.filter(option => option.toLowerCase().includes(filterValue) && !this.selectedOptions.includes(option));
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value && this.currentAllOptions.includes(value) && !this.selectedOptions.includes(value)) {
      this.selectedOptions.push(value);
      this.selectedOptionsIndex.push(this.currentAllOptions.indexOf(value));
    }
    event.chipInput!.clear();
    this.multiSelectControl.setValue(null);
  }

  remove(option: string): void {
    const index = this.selectedOptions.indexOf(option);
    if (index >= 0) {
      this.selectedOptions.splice(index, 1);
      this.selectedOptionsIndex.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;
    if (value && !this.selectedOptions.includes(value)) {
      this.selectedOptions.push(value);
      this.selectedOptionsIndex.push(this.currentAllOptions.indexOf(value));
    }
    this.multiSelectControl.setValue(null);
  }

  submitSymptoms() {
    const englishOptions = this.languageService.getLanguage() === 'ro' ? [] : this.selectedOptions;

    if(this.languageService.getLanguage() === 'ro')
    {
      for(let index of this.selectedOptionsIndex)
      {
        englishOptions.push(this.allOptions[index]);
      }
    }

    this.result = "...";
    this.symptomService.predictAsync(
      {
      symptoms: englishOptions,
      targetLanguage: this.languageService.getLanguage(),
      sourceLanguage: 'en'}
    ).then((result) => {
      this.result = result.disease;
    }).catch(() => {
      console.error('Error while predicting in component');
      this.result = 'Error while predicting';
    });
  }
}
