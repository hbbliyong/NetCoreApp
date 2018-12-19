using Microsoft.EntityFrameworkCore;
using NetCoreApp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1.Respository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        MySqlDbContext _context;
        public Repository(MySqlDbContext context)
        {
            this._context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.GetDbSet<T>().ToListAsync() ;
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).FirstOrDefaultAsync<T>();
        }
    }
}
