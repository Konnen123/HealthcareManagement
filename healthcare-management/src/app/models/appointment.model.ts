export interface Appointment
{
  id?: string,
  patientId: string,
  date: Date,
  startTime: string,
  endTime: string,
  userNotes?: string,
  doctorId: string,
}
