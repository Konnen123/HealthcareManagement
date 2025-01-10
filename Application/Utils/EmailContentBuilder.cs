namespace Application.Utils;

public static class EmailContentBuilder
{
    public static string Build(string firstName, string resetPasswordToken)
    {
        return $@"
            Hello {firstName},

            We received a request to reset your password. Use the link below to reset it:
            http://localhost:5072/reset-password?token={resetPasswordToken}

            If you didn’t ask to reset your password, you can safely ignore this email.

            Thanks,
            Your HealthCare Team";
    }
}