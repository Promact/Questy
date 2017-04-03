﻿using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Promact.Trappist.Repository.Tests
{
    public interface ITestsRepository
    {
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
      Task CreateTestAsync(Test test);
        /// <summary>
        /// this method is used to verify Name of the test is unique or not
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <returns>boolean</returns>
        Task<bool> IsTestNameUniqueAsync(string testName);       
        /// <summary>
        /// Method defined to fetch Tests from Test Model
        /// </summary>
        /// <returns>List Of Tests</returns>
        Task<List<Test>> GetAllTestsAsync();
    }
}