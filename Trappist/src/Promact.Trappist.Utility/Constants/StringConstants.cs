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
        public string UserName
        {
            get
            {
                return "Trappist123@gmail.com";
            }
        }
        public string FailedToSendEmailError
        {
            get
            {
                return "Failed to send the mail! Please try again";
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
                return "Password must be alphanumeric including at least 1 uppercase letter,1 lowercase letter and a special character with 8 to 14 characters";
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

        public string CategoryExistInQuestionError
        {
            get
            {
                return "This Category belongs to Questions. You can not delete this Category.";
            }
        }

        public string CategoryExistInTestError
        {
            get
            {
                return "This Category exist in a test";
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

        public string WarningMessage
        {
            get
            {
                return "Your test is about to end. Hurry up!!";
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
                return "Question exist in Test. Question can not be deleted.";
            }
        }

        public string QuestionEditError
        {
            get
            {
                return "Question exist in a Test.";
            }
        }
        #endregion

        #region "Test-Question-Selection"
        public string SuccessfullySaved
        {
            get
            {
                return "Your changes saved successfully";
            }
        }
        public string NoNewChanges
        {
            get
            { 
                return "No new questions selected.."; 
            }
        }
        #endregion

        #region Session Keys
        public string AttendeeIdSessionKey
        {
            get
            {
                return "ATTENDEE_ID";
            }
        }

        public string TestLinkSessionKey
        {
            get
            {
                return "MAGIC_LINK";
            }
        }

        public string Path
        {
            get
            {
                return "Path";
            }
        }
        #endregion

        #region Code Response
        public string AllTestCasePassed
        {
            get
            {
                return "Congratulations! All test cases passed.";
            }
        }

        public string DefaultTestCasePassed
        {
            get
            {
                return "Default test case passed.";
            }
        }

        public string SomeTestCasePassed
        {
            get
            {
                return "Wrong Answer. Some test cases failed.";
            }
        }

        public string NoTestCasePassed
        {
            get
            {
                return "Wrong Answer. None of the test cases passed.";
            }
        }

        public string DefaultTestCaseFailed
        {
            get
            {
                return "Default test case failed.";
            }
        }
        #endregion
    }
}