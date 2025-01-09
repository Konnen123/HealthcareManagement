import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {MatFormField, MatHint, MatLabel, MatSuffix} from '@angular/material/form-field';
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from '@angular/material/datepicker';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef} from '@angular/material/dialog';
import {MatIcon} from '@angular/material/icon';
import {LanguageService} from '../../services/language/language.service';
import {TranslatePipe} from '@ngx-translate/core';


@Component({
  selector: 'app-filter-dialog',
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatDatepickerToggle,
    MatDatepicker,
    MatInput,
    MatButton,
    MatDatepickerInput,
    MatLabel,
    MatIcon,
    MatSuffix,
    MatHint,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    TranslatePipe
  ],
  templateUrl: './filter-dialog.component.html',
  styleUrl: './filter-dialog.component.scss'
})
export class FilterDialogComponent implements OnInit{
  filterForm: FormGroup;

  constructor(
    readonly fb: FormBuilder,
    readonly dialogRef: MatDialogRef<FilterDialogComponent>,
    readonly languageService: LanguageService
  ) {
    this.filterForm = this.fb.group({
      startTime: [''],
      date: [''],
    });
  }

  ngOnInit(): void
  {
    this.languageService.setLanguage();
  }

  applyFilters() {
    if (this.filterForm.valid) {
      this.dialogRef.close(this.filterForm.value);
    }
  }
}
