namespace Promact.Trappist.Utility.Constants
{
    public class StringConstants : IStringConstants
    {       
        public string InvalidTestName
        {
            get
            {
                return "Invalid Test Name "; //string returned when an invalid test name is entered
            }
        }

        public string Success
        {
            get
            {
                return "Test Created successfuly"; //string returned when a test is created successfuly
            }
        }
    }
}
