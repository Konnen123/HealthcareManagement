import {AbstractControl, ValidationErrors} from '@angular/forms';

export class CustomValidators {
  static isValidGuid(control: AbstractControl): ValidationErrors | null {
    const guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(control.value) ? null : {invalidGuid: true};
  }

  static isNotEmptyGuid(control: AbstractControl): ValidationErrors | null {
    return control.value !== '00000000-0000-0000-0000-000000000000'
      ? null
      : { emptyGuid: true };
  }

  static endTimeAfterStartTime(startTimeControlName: string): (control: AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      const formGroup = control.parent;
      if (!formGroup) {
        return null;
      }
      const startTime = formGroup.get(startTimeControlName)?.value;
      const endTime = control.value;

      if (!startTime || !endTime) {
        return null;
      }
      return endTime > startTime ? null : { endTimeBeforeStartTime: true }; // Validation error
    };
  }

  static isValidDate(control: AbstractControl): ValidationErrors | null {
    const date = new Date(control.value);
    return !isNaN(date.getTime()) ? null : { invalidDate: true };
  }

  static isNotPastDate(control: AbstractControl): ValidationErrors | null {
    // console.log('Control value:', new Date(control.value));
    const date = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const isValid = date.getTime() >= today.getTime();
    //console.log('Is valid:', isValid); // Check comparison result
    return isValid ? null : { pastDate: true };
  }

  static passwordsMatch(passwordControlName: string): (control: AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      const parent = control.parent;
      if (!parent) {
        return null;
      }

      const password = parent.get(passwordControlName)?.value;
      const confirmPassword = control.value;

      return password && confirmPassword && password !== confirmPassword
        ? { passwordsMismatch: true }
        : null;
    };
  }

  static dateOfBirthValidator(control: AbstractControl): ValidationErrors | null {
    const dateOfBirth = control.value;

    if (!dateOfBirth) {
      return null;
    }

    const inputDate = new Date(dateOfBirth);
    const today = new Date();
    const hundredYearsAgo = new Date(today.getFullYear() - 100, today.getMonth(), today.getDate());

    if (inputDate < hundredYearsAgo) {
      return { tooOld: true };
    }

    if (inputDate > today) {
      return { futureDate: true };
    }

    return null;
  }
}
