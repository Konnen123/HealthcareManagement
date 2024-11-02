using Domain.Utils;

namespace Domain.Errors;

public static class AppointmentErrors
{
    public static Error CreateFailed(string description) => new Error("Appointment.CreateFailed", description);
    public static Error NotFound(Guid guid) => new Error("Appointment.NotFound", $"The appointment with id {guid} was not found");
    public static Error GetFailed(string description) => new Error("Appointment.GetFailed", description);
    public static Error NotAllowedToCancel(string description) => new Error("Appointment.NotAllowedToCancel", description);
    public static Error CancelFailed(string description) => new Error("Appointment.CancelFailed", description);
    public static Error AlreadyCanceled(string description) => new Error("Appointment.AlreadyCanceled", description);
}