using Domain.Utils;

namespace Domain.Errors;

public static class AppointmentErrors
{
    public static Error NotAllowedToCancel(string description) => new Error("Appointment.NotAllowedToCancel", description);

    public static Error CancelFailed(string description) => new Error("Appointment.CancelFailed", description);
    public static Error AlreadyCanceled(string description) => new Error("Appointment.AlreadyCanceled", description);
}