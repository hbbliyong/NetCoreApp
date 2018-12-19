using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApp.Model;
using WebApplication1.Respository;

namespace WebApplication1.Services
{
    public class UserService:IUserService
    {
        private IRepository<User> repository;
        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public Task<List<User>> GetUsers()
        {
            return repository.GetAllAsync();
        }
    }
}
