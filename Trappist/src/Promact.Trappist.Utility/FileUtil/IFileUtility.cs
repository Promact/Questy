namespace Promact.Trappist.Utility.FileUtil
{
    public interface IFileUtility
    {
        /// <summary>
        /// This method used for write json file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="jsonString"></param>
        /// <returns>It returns true if file successfully writes json file else returns false.</returns>
        bool WriteJson(string path, string jsonString);
    }
}
