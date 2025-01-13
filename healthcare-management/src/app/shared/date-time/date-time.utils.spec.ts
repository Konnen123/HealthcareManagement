import { formatDate, formatTime } from './date-time.utils';

describe('Date-Time Utils', () => {
  describe('formatDate', () => {
    it('should format a valid date object to YYYY-MM-DD', () => {
      const date = new Date(2024, 0, 1); // January 1, 2024
      expect(formatDate(date)).toBe('2024-01-01');
    });

    it('should format a valid date string to YYYY-MM-DD', () => {
      const date = '2024-01-01T10:00:00Z';
      expect(formatDate(date)).toBe('2024-01-01');
    });

    it('should return an empty string if date is null', () => {
      expect(formatDate(null as any)).toBe('');
    });

    it('should return an empty string if date is undefined', () => {
      expect(formatDate(undefined as any)).toBe('');
    });

    it('should return an empty string if the date is invalid', () => {
      expect(formatDate('invalid-date')).toBe('');
    });
  });

  describe('formatTime', () => {
    it('should format a valid time string (HH:mm:ss) to HH:mm', () => {
      const time = '10:30:45';
      expect(formatTime(time)).toBe('10:30');
    });

    it('should format a valid time string (HH:mm) to HH:mm', () => {
      const time = '10:30';
      expect(formatTime(time)).toBe('10:30');
    });

    it('should return an empty string if time is null', () => {
      expect(formatTime(null as any)).toBe('');
    });

    it('should return an empty string if time is undefined', () => {
      expect(formatTime(undefined as any)).toBe('');
    });

    it('should return an empty string if time is invalid', () => {
      expect(formatTime('invalid-time')).toBe('');
    });
  });
});
