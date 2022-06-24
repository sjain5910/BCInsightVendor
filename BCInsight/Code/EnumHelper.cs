namespace BCInsight.Code
{
    public class EnumHelper
    {
    }

    public enum EmailMessageType
    {
        forgotpassword = 1,
        Register = 2,
        ChangedEmail = 3,
        SendAdminForgotPasswordRequest = 4,
        OTPVarification = 5
    };
    public enum NotificationEnum
    {
        Success,
        Error,
        Warning,
        Info
    }
}