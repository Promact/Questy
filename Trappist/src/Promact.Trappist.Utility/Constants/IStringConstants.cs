﻿namespace Promact.Trappist.Utility.Constants
{
    public interface IStringConstants
    {
        /// property InvalidTestName is called when an invalid test name is entered at the time of test creation      
        string InvalidTestName { get; }

        /// property Success is called when test creation is successfuly done
        string Success { get; }

        #region BasicSetup Constants        
        /// <summary>
        /// property SetupConfigFilename is called whenever required SetupConfig file name
        /// </summary>
        string SetupConfigFileName { get; }
        /// <summary>
        /// this property used for provide body constant in mail.
        /// </summary>
        string TestMailBody { get; }
        /// <summary>
        /// this property used for provide subject constant in mail.
        /// </summary>
        string TestMailSubject { get; }

        #endregion

        #region "Account Constants"
        /// <summary>
        /// property InavalidLoginError is used when input credentials are not matched with database at the time of login
        /// </summary>
        string InavalidLoginError { get; }
        /// <summary>
        /// property InavalidLoginError is used when model state and model binding are not proper
        /// </summary>
        string InavalidModelError { get; }
        /// <summary>
        /// property InvalidEmailError is used when emailid does not match with databse id
        /// </summary>
        string InvalidEmailError { get; }
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
    }
}
