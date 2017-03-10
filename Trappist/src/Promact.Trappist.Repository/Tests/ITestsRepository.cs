using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.Repository.Tests
{
    public interface ITestsRepository
    {
        void createTest(Test t1);// for test creation
        bool uniqueName(Test t); //to check whether test name is unique or not

    }

}