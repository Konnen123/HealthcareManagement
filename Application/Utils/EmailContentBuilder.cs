namespace Application.Utils;

public static class EmailContentBuilder
{
    public static string BuildResetPasswordEmail(string firstName, string resetPasswordToken)
    {
        return $@"
            Hello {firstName},

            We received a request to reset your password. Use the link below to reset it:
            http://localhost:4200/reset-password?token={resetPasswordToken}

            If you didn’t ask to reset your password, you can safely ignore this email.

            Thanks,
            Your HealthCare Team";
    }
    public static string BuildVerificationEmail(string firstName, string verificationToken)
    {
        return $@"
            Hello {firstName},

            Welcome to HealthCare! To complete your registration, please verify your email address by clicking the link below:
            https://localhost:7121/api/v1/Auth/verify-email?token={verificationToken}

            If you didn’t create an account with us, you can safely ignore this email.

            Thanks,
            Your HealthCare Team";
    }
}