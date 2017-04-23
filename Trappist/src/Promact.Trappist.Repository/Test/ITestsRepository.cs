using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.Repository.Tests
{
    public interface ITestsRepository
    {
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        void CreateTest(Test test);
        /// <summary>
        /// this method is used to verify Name of the test is unique or not
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <returns>boolean</returns>
        bool UniqueTestName(Test test); 

    }

}