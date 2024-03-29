﻿namespace Promact.Trappist.Utility.Constants
{
    public interface IStringConstants
    {
        /// property InvalidTestName is called when an invalid test name is entered at the time of test creation      
        string InvalidTestName { get; }

        /// property Success is called when test creation is successfully done
        string Success { get; }

        #region BasicSetup Constants  

        /// <summary>
        /// Property SetupConfigFilename is called whenever required SetupConfig file name
        /// </summary>
        string SetupConfigFileName { get; }

        /// <summary>
        /// This property used for provide body constant in mail.
        /// </summary>
        string TestMailBody { get; }

        /// <summary>
        /// This property used for provide subject constant in mail.
        /// </summary>
        string TestMailSubject { get; }

        /// <summary>
        /// This property used for providing error message if a user already exists.
        /// </summary>
        string UserAlreadyExistErrorMessage { get; }

        /// <summary>
        /// This property used for providing error message if any issues in the database.
        /// </summary>
        string DatabaseRelatedIssue { get; }
        #endregion

        /// <summary>
        /// property to store characters for random link generation
        /// </summary>
        string CharactersForLink { get; }

        #region "Account Constants"

        /// <summary>
        /// property InvalidLoginError is used when input credentials are not matched with database at the time of login
        /// </summary>
        string InvalidLoginError { get; }

        /// <summary>
        /// property InvalidLoginError is used when model state and model binding are not proper
        /// </summary>
        string InvalidModelError { get; }

        /// <summary>
        /// property InvalidEmailError is used when emailid does not match with databse id
        /// </summary>
        string InvalidEmailError { get; }

        /// <summary>
        /// property InvalidTokenError is used when user will try to reset password with already used link
        /// </summary>
        string InvalidTokenError { get; }

        /// <summary>
        /// Property UserName to return "Trappist123@gmail.com" string
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Property Name to return "Questy Test User" string
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property FailedTosendEmailError is used when mail cannot be send due to server connection problem 
        /// </summary>
        string FailedToSendEmailError { get; }
        #endregion

        #region Profile Constants
        /// <summary>
        /// property is used when the old passowrd given by the user doesnot match with the password in the database
        /// </summary>
        string InvalidOldPasswordError { get; }

        /// <summary>
        /// property is used when the format of the password is wrong
        /// </summary>
        string InvalidPasswordFormatError { get; }
        #endregion

        #region "Category Constants"

        /// <summary>
        /// Property ErrorKey is key of error message
        /// </summary>
        string ErrorKey { get; }

        /// <summary>
        /// Property CategoryNameExistsError is used to check Category Name uniqueness
        /// </summary>
        string CategoryNameExistsError { get; }

        /// <summary>
        /// Property CategoryExistInQuestion is used to check Category exist in Question or not
        /// </summary>
        string CategoryExistInQuestionError { get; }

        /// <summary>
        /// Property CategoryExistInQuestion is used to check Category exist in Question or not
        /// </summary>
        string CategoryExistInTestError { get; }
        #endregion

        #region "TestSettings Constants"

        /// <summary>
        /// Property TestNameExistsError is used to check Test Name uniqueness
        /// </summary>
        string TestNameExistsError { get; }

        /// <summary>
        /// Property TestNameInvalidError is used to check Test Name is not null and it contains valid characters
        /// </summary>
        string TestNameInvalidError { get; }

        /// <summary>
        /// Property WarningMessage is used to set the default warning message
        /// </summary>
        string WarningMessage { get; }
        #endregion

        #region TestConduct Constants
        /// <summary>
        /// This property contain magic string of test.
        /// </summary>
        string MagicString { get; }
        #endregion

        #region "Question Constants"
        /// <summary>
        /// Property QuestionExistInTestError is used when Question exist in Test
        /// </summary>
        string QuestionExistInTestError { get; }

        /// <summary>
        /// Property QuestionExistInTestError is used when Question exist in Test
        /// </summary>
        string QuestionEditError { get; }
        #endregion

        #region "Test-Question-Selection"
        /// <summary>
        /// This property defines that the questions are saved successfully to test
        /// </summary>
        string SuccessfullySaved { get; }
        /// <summary>
        /// This propert defines that user has not selected any new question to add in test
        /// </summary>
        string NoNewChanges { get; }

        #endregion

        #region Session Key
        /// <summary>
        /// Session Key to get or set Attendee Id
        /// </summary>
        string AttendeeIdSessionKey { get; }

        string Path { get; }


        #endregion

        #region Code Response
        string AllTestCasePassed { get; }

        string DefaultTestCasePassed { get; }

        string SomeTestCasePassed { get; }

        string NoTestCasePassed { get; }

        string DefaultTestCaseFailed { get; }
        #endregion
    }
}