namespace Promact.Trappist.Utility.Constants
{
    public class StringConstants : IStringConstants
    {
        public string InvalidTestName
        {
            get
            {
                return "Invalid Test Name ";
            }
        }

        public string Success
        {
            get
            {
                return "Test Created successfuly";
            }
        }

        #region Setup Constants
        public string SetupConfigFileName
        {
            get
            {
                return "setup.json";
            }
        }

        public string TestMailBody
        {
            get
            {
                return "Test Mail Settings";
            }
        }

        public string TestMailSubject
        {
            get
            {
                return "Test mail Subject";
            }
        }
        #endregion

        #region "Account Constants"

        public string InavalidLoginError
        {
            get
            {
                return "Username or Password Is Invalid!";
            }
        }

        public string InavalidModelError
        {
            get
            {
                return "Invalid Login Attempt!";
            }
        }
        public string InvalidEmailError
        {
            get
            {
                return "Incorrect Input! Please Enter Proper Email Id";
            }
        }
        #endregion

        #region Profile Constants
        public string InvalidOldPasswordError
        {
            get
            {
                return "Your current password is wrong";
            }
        }
        public string InvalidPasswordFormatError
        {
            get
            {
                return "Password must be alphanumeric including at least 1 uppercase letter and a special character";
            }
        }
        #endregion
    }
}