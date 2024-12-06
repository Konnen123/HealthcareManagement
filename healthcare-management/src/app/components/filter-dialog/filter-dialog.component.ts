import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {MatFormField, MatHint, MatLabel, MatSuffix} from '@angular/material/form-field';
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from '@angular/material/datepicker';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef} from '@angular/material/dialog';
import {MatIcon} from '@angular/material/icon';


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
    MatDialogClose
  ],
  templateUrl: './filter-dialog.component.html',
  styleUrl: './filter-dialog.component.scss'
})
export class FilterDialogComponent {
  filterForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<FilterDialogComponent>
  ) {
    this.filterForm = this.fb.group({
      startTime: [''],
      date: [''],
    });
  }

  applyFilters() {
    if (this.filterForm.valid) {
      //console.log(this.filterForm.value);
      this.dialogRef.close(this.filterForm.value);
    }
  }
}
