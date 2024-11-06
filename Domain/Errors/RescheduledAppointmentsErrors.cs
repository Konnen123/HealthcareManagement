using Domain.Utils;

namespace Domain.Errors;

public static class RescheduledAppointmentsErrors
{
    public static Error AlreadyRescheduled(string description) => new Error("RescheduledAppointments.AlreadyRescheduled", description);
    public static Error NotAllowedToReschedule(string description) => new Error("RescheduledAppointments.NotAllowedToReschedule", description);
}