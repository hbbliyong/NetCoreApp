using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.model;
using WebApplication1.Respository;
namespace WebApplication1.Services
{
    public class TestService : ITestService
    {
        private IRepository<Test> _repository;
        public TestService(IRepository<Test> repository)
        {
            _repository = repository;
        }
        public Task<List<Test>> GetTests()
        {
            return _repository.GetAllAsync();
        }
    }
}
