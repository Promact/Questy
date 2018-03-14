namespace Promact.Trappist.Utility.GlobalUtil
{
    public interface IGlobalUtil
    {
        /// <summary>
        /// this method is used to generate a random string which is unique for every test
        /// </summary>      
        /// <param name="length">length of the random string</param>
        string GenerateRandomString(int length);
    }
}
