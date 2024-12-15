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
import {SymptomService} from '../../services/symptom-checker/symptom-checker.service';

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
  ],
  templateUrl: './symptom-checker.component.html',
  styleUrl: './symptom-checker.component.scss'
})
export class SymptomCheckerComponent implements OnInit{
  separatorKeysCodes: number[] = [ENTER, COMMA];
  multiSelectControl = new FormControl('');
  allOptions: string[] = symptoms.symptoms;
  filteredOptions: Observable<string[]> = of([]);
  selectedOptions: string[] = [];
  result: string = '';

  constructor(
    readonly symptomService: SymptomService,
  ) {}

  ngOnInit() {
      this.filteredOptions = this.multiSelectControl.valueChanges.pipe(
        startWith(''),
        map((value: string | null) => this._filter(value))
      );
  }

  private _filter(value: string | null): string[] {
    const filterValue = (value ?? '').toLowerCase();
    return this.allOptions.filter(option => option.toLowerCase().includes(filterValue) && !this.selectedOptions.includes(option));
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value && this.allOptions.includes(value) && !this.selectedOptions.includes(value)) {
      this.selectedOptions.push(value);
    }
    event.chipInput!.clear();
    this.multiSelectControl.setValue(null);
  }

  remove(option: string): void {
    const index = this.selectedOptions.indexOf(option);
    if (index >= 0) {
      this.selectedOptions.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;
    if (value && !this.selectedOptions.includes(value)) {
      this.selectedOptions.push(value);
    }
    this.multiSelectControl.setValue(null);
  }

  submitSymptoms() {
    console.log('Selected symptoms:', this.selectedOptions);
    this.result = "sore throat";
    this.symptomService.predictAsync(this.selectedOptions).then((result) => {
      this.result = result.disease;
    }).catch(() => {
      console.error('Error while predicting in component');
      this.result = 'Error while predicting';
    });
  }
}
