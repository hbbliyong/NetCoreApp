using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1.Respository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        Md5Context _context;
        public Repository(Md5Context context)
        {
            this._context = context;
        }

        public Task<List<T>> GetAllAsync()
        {
            return _context.Set<T>().ToListAsync();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).FirstOrDefaultAsync<T>();
        }
    }
}
