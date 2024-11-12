export interface Appointment
{
  id?: string,
  patientId: string,
  date: Date,
  starTime: string,
  endTime: string,
  userNotes?: string,
  doctorId: string,
}
