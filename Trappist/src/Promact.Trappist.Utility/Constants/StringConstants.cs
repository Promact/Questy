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
        /// <summary>
        /// property setup.json Filename is called whenever required setup.json file name
        /// </summary>
        public string SetupConfigFileName
        {
            get
            {
                return "setup.json";
            }
        }
        /// <summary>
        ///  this property used for provide body constant in mail.
        /// </summary>
        public string BodyOfMail
        {
            get
            {
                return "Test Mail Settings";
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
        #endregion
    }
}
