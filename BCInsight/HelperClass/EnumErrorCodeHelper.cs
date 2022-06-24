namespace BCInsight.Web.HelperClass
{
    public class EnumErrorCodeHelper
    {
        public enum SuccessCodes
        {
            Success = 0,
            LoginSuccess = 1,
            VerifySuccess = 2,
            ResendSuccess = 3,
            RegisterSuccess = 4,
            MAIL_SEND_SUCCESS = 5,
            PODCAST_ADDED_SUCCESS = 6,
            USER_VARIFICATION_SUCCESS = 7,
            UserNotActive = 8,
        }

        public enum ErrorCodes
        {
            Fail = 1000,
            LoginFail = 1001,
            InvalidDeviceRequest = 1002,
            InvalidModel = 1003,
            alreadyRegister = 1004,
            USERNAME_NOTFOUND = 1005,
            ENTER_VALID_OTP = 1006,
            PODCAST_NOTFOUND = 1007,
        }
    }
}