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
        public string UserAlreadyExistErrorMessage
        {
            get
            {
                return "User already Exists.";
            }
        }
        public string DatabaseRelatedIssue
        {
            get
            {
                return "There is some issue in existing database or you don't have permission to access database.";
            }
        }
        #endregion

        public string CharactersForLink
        {
            get
            {
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            }
        }

        #region "Account Constants"

        public string InvalidLoginError
        {
            get
            {
                return "Username or Password Is Invalid!";
            }
        }

        public string InvalidModelError
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
        public string InvalidTokenError
        {
            get
            {
                return "Invalid Link To Reset The Password!";
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

        #region "Category Constants"
        public string CategoryNameExistsError
        {
            get
            {
                return "Category Name Already Exists";
            }
        }

        public string ErrorKey
        {
            get
            {
                return "error";
            }
        }
        #endregion

        #region "TestSettings Constants"
        public string TestNameExistsError
        {
            get
            {
                return "Test Name Already Exists";
            }
        }

        public string TestNameInvalidError
        {
            get
            {
                return "Test Name is Invalid";
            }
        }
        #endregion

        #region TestConduct Constants
        public string MagicString
        {
            get
            {
                return "H0SGXPOwsR";
            }
        }
        #endregion

        #region "Question Constants"
        public string QuestionExistInTestError
        {
            get
            {
                return "Question exist in Test.Question cannot be deleted";
            }
        }
        #endregion
    }
}