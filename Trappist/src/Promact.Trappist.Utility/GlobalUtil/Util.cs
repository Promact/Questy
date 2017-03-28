using Promact.Trappist.Utility.Constants;
using System;
using System.Linq;

namespace Promact.Trappist.Utility.GlobalUtil
{
    public class Util : IUtil
    {
        private static Random random = new Random();
        private readonly IStringConstants _stringConstants;
        public Util(IStringConstants stringConstants)
        {
            _stringConstants = stringConstants;
        }
        public string RandomLinkGenerator(int length)
        {
            string charactersForRandomString = _stringConstants.CharactersForLink;
            var link = new string(Enumerable.Repeat(charactersForRandomString, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return link;
        }       
    }
}
