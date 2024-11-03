using Domain.Utils;

namespace Domain.Errors;

public static class RescheduledAppointmentsErrors
{
    public static Error AlreadyRescheduled(string description) => new Error("RescheduledAppointments.AlreadyRescheduled", description);
    public static Error GetFailed(string description) => new Error("RescheduledAppointments.GetFailed", description);
    public static Error NotAllowedToReschedule(string description) => new Error("RescheduledAppointments.NotAllowedToReschedule", description);
    public static Error CreateFailed(string description) => new Error("RescheduledAppointments.CreateFailed", description);
    public static Error NotFound(Guid guid) => new Error("RescheduledAppointments.NotFound", $"The appointment request with id {guid} was not found");
    public static Error DeleteFailed(string description) => new Error("RescheduledAppointments.DeleteFailed", description);
}