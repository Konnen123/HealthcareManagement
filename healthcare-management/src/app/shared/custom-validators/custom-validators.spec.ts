import { CustomValidators } from './custom-validators';
import { FormControl, FormGroup } from '@angular/forms';

describe('CustomValidators', () => {
  it('should validate a valid GUID', () => {
    const control = new FormControl('123e4567-e89b-12d3-a456-426614174000');
    const result = CustomValidators.isValidGuid(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate an invalid GUID', () => {
    const control = new FormControl('invalid-guid');
    const result = CustomValidators.isValidGuid(control);
    expect(result).toEqual({ invalidGuid: true }); // Validation error
  });

  it('should validate a non-empty GUID', () => {
    const control = new FormControl('123e4567-e89b-12d3-a456-426614174000');
    const result = CustomValidators.isNotEmptyGuid(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate an empty GUID', () => {
    const control = new FormControl('00000000-0000-0000-0000-000000000000');
    const result = CustomValidators.isNotEmptyGuid(control);
    expect(result).toEqual({ emptyGuid: true }); // Validation error
  });

  it('should validate endTime after startTime', () => {
    const formGroup = new FormGroup({
      startTime: new FormControl('10:00'),
      endTime: new FormControl('11:00'),
    });
    const control = formGroup.get('endTime')!;
    const validator = CustomValidators.endTimeAfterStartTime('startTime');
    const result = validator(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate endTime before startTime', () => {
    const formGroup = new FormGroup({
      startTime: new FormControl('11:00'),
      endTime: new FormControl('10:00'),
    });
    const control = formGroup.get('endTime')!;
    const validator = CustomValidators.endTimeAfterStartTime('startTime');
    const result = validator(control);
    expect(result).toEqual({ endTimeBeforeStartTime: true }); // Validation error
  });

  it('should validate a valid date', () => {
    const control = new FormControl('2024-01-01');
    const result = CustomValidators.isValidDate(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate an invalid date', () => {
    const control = new FormControl('invalid-date');
    const result = CustomValidators.isValidDate(control);
    expect(result).toEqual({ invalidDate: true }); // Validation error
  });

  it('should validate a date not in the past', () => {
    const today = new Date().toISOString().split('T')[0]; // Current date in YYYY-MM-DD format
    const control = new FormControl(today);
    const result = CustomValidators.isNotPastDate(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate a date in the past', () => {
    const pastDate = new Date(Date.now() - 86400000).toISOString().split('T')[0]; // Yesterday's date
    const control = new FormControl(pastDate);
    const result = CustomValidators.isNotPastDate(control);
    expect(result).toEqual({ pastDate: true }); // Validation error
  });

  it('should validate matching passwords', () => {
    const formGroup = new FormGroup({
      password: new FormControl('password123'),
      confirmPassword: new FormControl('password123'),
    });
    const control = formGroup.get('confirmPassword')!;
    const validator = CustomValidators.passwordsMatch('password');
    const result = validator(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate non-matching passwords', () => {
    const formGroup = new FormGroup({
      password: new FormControl('password123'),
      confirmPassword: new FormControl('password456'),
    });
    const control = formGroup.get('confirmPassword')!;
    const validator = CustomValidators.passwordsMatch('password');
    const result = validator(control);
    expect(result).toEqual({ passwordsMismatch: true }); // Validation error
  });

  it('should validate a valid date of birth', () => {
    const control = new FormControl('2000-01-01');
    const result = CustomValidators.dateOfBirthValidator(control);
    expect(result).toBeNull(); // No validation error
  });

  it('should invalidate a future date of birth', () => {
    const futureDate = new Date(Date.now() + 86400000).toISOString().split('T')[0];
    const control = new FormControl(futureDate);
    const result = CustomValidators.dateOfBirthValidator(control);
    expect(result).toEqual({ futureDate: true }); // Validation error
  });

  it('should invalidate a date of birth older than 100 years', () => {
    const oldDate = new Date(1900, 0, 1).toISOString().split('T')[0];
    const control = new FormControl(oldDate);
    const result = CustomValidators.dateOfBirthValidator(control);
    expect(result).toEqual({ tooOld: true }); // Validation error
  });
});
