using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Promact.Trappist.Utility.FileUtil
{
    [ExcludeFromCodeCoverage]
    public class FileUtility : IFileUtility
    {
        #region IFileUtility method
        public bool WriteJson(string path, string jsonString)
        {
            try
            {
                File.WriteAllText(path, jsonString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
