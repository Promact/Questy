namespace Promact.Trappist.Utility.GlobalUtil
{
    public interface IUtil
    {
        /// <summary>
        /// this method is used to generate a random string which is unique for every test
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <param name="length">length of the random string</param>
        string RandomLinkGenerator(int length);
    }
}
