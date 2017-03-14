using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.Repository.Tests
{
    public interface ITestsRepository
    {
        void CreateTest(Test test);// for test creation
        bool UniqueName(Test test); //to check whether test name is unique or not

    }

}