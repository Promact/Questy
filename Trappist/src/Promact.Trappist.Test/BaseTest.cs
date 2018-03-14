using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.DbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promact.Trappist.Test
{
    /// <summary>
    /// Base unit test class boilerplate
    /// Initializes/Disposes scope for each test case, clears and seeds db
    /// </summary>
    public class BaseTest : IDisposable
    {
        protected readonly IServiceScope _scope;
        protected readonly TrappistDbContext _trappistDbContext;

        public BaseTest(Bootstrap bootstrap)
        {
            _scope = bootstrap.ServiceProvider.CreateScope();
            _trappistDbContext = _scope.ServiceProvider.GetService<TrappistDbContext>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
