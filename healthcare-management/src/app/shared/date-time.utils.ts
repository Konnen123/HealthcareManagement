export function formatDate(date: Date | string): string {
  if (!date) {
    return '';
  }
  const d = new Date(date);
  const year = d.getFullYear();
  const month = (d.getMonth() + 1).toString().padStart(2, '0');
  const day = d.getDate().toString().padStart(2, '0');
  return `${year}-${month}-${day}`;
}

export function formatTime(time: string): string {
  if (!time) {
    return '';
  }
  const [hours, minutes] = time.split(':');
  return `${hours}:${minutes}`;
}
