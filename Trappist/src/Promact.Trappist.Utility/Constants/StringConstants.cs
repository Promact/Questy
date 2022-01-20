namespace Promact.Trappist.Utility.Constants
{
    public class StringConstants : IStringConstants
    {
        public string InvalidTestName => "Invalid Test Name ";

        public string Success => "Test Created successfully";

        #region Setup Constants
        public string SetupConfigFileName => "setup.json";

        public string TestMailBody => "Test Mail Settings";

        public string TestMailSubject => "Test mail Subject";

        public string UserAlreadyExistErrorMessage => "User already Exists.";

        public string DatabaseRelatedIssue => "There is some issue in existing database or you don't have permission to access database.";

        #endregion

        public string CharactersForLink => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #region "Account Constants"

        public string InvalidLoginError => "Username or Password Is Invalid!";

        public string InvalidModelError => "Invalid Login Attempt!";

        public string InvalidEmailError => "Incorrect Input! Please Enter Proper Email Id";

        public string InvalidTokenError => "Invalid Link To Reset The Password!";

        public string UserName => "Trappist123@gmail.com";

        public string Name => "Questy Test User";

        public string FailedToSendEmailError => "Failed to send the mail! Please try again";

        #endregion

        #region Profile Constants
        public string InvalidOldPasswordError => "Your current password is wrong";

        public string InvalidPasswordFormatError => "Password must be alphanumeric including at least 1 uppercase letter,1 lowercase letter and a special character with 8 to 14 characters";

        #endregion

        #region "Category Constants"
        public string CategoryNameExistsError => "Category Name Already Exists";

        public string ErrorKey => "error";

        public string CategoryExistInQuestionError => "Section contains questions. You can not delete this Section.";

        public string CategoryExistInTestError => "This Category exist in a test";

        #endregion

        #region "TestSettings Constants"
        public string TestNameExistsError => "Test Name Already Exists";

        public string TestNameInvalidError => "Test Name is Invalid";

        public string WarningMessage => "Your test is about to end. Hurry up!!";

        #endregion

        #region TestConduct Constants
        public string MagicString => "H0SGXPOwsR";

        #endregion

        #region "Question Constants"
        public string QuestionExistInTestError => "Question exist in Test. Question can not be deleted.";

        public string QuestionEditError => "Question exist in a Test.";

        #endregion

        #region "Test-Question-Selection"
        public string SuccessfullySaved => "Your changes saved successfully";

        public string NoNewChanges => "No new questions selected..";

        #endregion

        #region Session Keys
        public string AttendeeIdSessionKey => "ATTENDEE_ID";

        public string TestLinkSessionKey => "MAGIC_LINK";

        public string Path => "Path";

        #endregion

        #region Code Response
        public string AllTestCasePassed => "Congratulations! All test cases passed.";

        public string DefaultTestCasePassed => "Default test case passed.";

        public string SomeTestCasePassed => "Wrong Answer. Some test cases failed.";

        public string NoTestCasePassed => "Wrong Answer. None of the test cases passed.";

        public string DefaultTestCaseFailed => "Default test case failed.";

        #endregion
    }
}