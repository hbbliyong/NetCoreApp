using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.model;

namespace WebApplication1.Services
{
   public interface IUserService
    {
        Task<List<User>> GetUsers();
    }
}
