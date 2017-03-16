
using Promact.Trappist.DomainModel.Models.Test;
using System.Collections.Generic;


namespace Promact.Trappist.Repository.TestDashBoard
{
    public interface ITestDashBoardRepository
    {
        /// <summary>
        /// Method defined to fetch Tests from Test Model
        /// </summary>
        /// <returns>List Of Tests</returns>
        List<Test> GetAllTests();


    }
}