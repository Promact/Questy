using System;

namespace Promact.Trappist.Utility.Constants
{
    public interface IStringConstants
    {
        /// property InvalidTestName is called when an invalid test name is entered at the time of test creation      
        string InvalidTestName { get; }

        /// property Success is called when test creation is successfuly done
        string Success { get; }

        #region "Account Constants"
        /// <summary>
        /// property InavalidLoginError is used when input credentials are not matched with database at the time of login
        /// </summary>
        string InavalidLoginError { get;  }
        /// <summary>
        /// property InavalidLoginError is used when model state and model binding are not proper
        /// </summary>
        string InavalidModelError { get;  }
        #endregion
    }
}
