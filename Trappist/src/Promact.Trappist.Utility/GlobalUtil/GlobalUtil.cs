using Promact.Trappist.Utility.Constants;
using System;
using System.Linq;

namespace Promact.Trappist.Utility.GlobalUtil
{
    public class GlobalUtil : IGlobalUtil
    {
        private readonly IStringConstants _stringConstants;
        public GlobalUtil(IStringConstants stringConstants)
        {
            _stringConstants = stringConstants;
        }
        public string GenerateRandomString(int length)
        {
            var random = new Random();
            string charactersForRandomString = _stringConstants.CharactersForLink;
            //generating random string by using a predefined set of characters, random.Next selects the next element in the link from the set of characters provided.
            var randomString = new string(Enumerable.Repeat(charactersForRandomString, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
