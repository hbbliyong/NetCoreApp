﻿using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using NetCoreApp.Model;
using NetCoreApp.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace NetCoreApp.Db
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseDbContext : DbContext, IDbContextCore
    {
        protected readonly DbContextOption _option;
        public DatabaseFacade GetDatabase() => Database;

        public BaseDbContext(IOptions<DbContextOption> option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            if (string.IsNullOrEmpty(option.Value.ConnectionString))
                throw new ArgumentNullException(nameof(option.Value.ConnectionString));
            //if (string.IsNullOrEmpty(option.Value.ModelAssemblyName))
            //    throw new ArgumentNullException(nameof(option.Value.ModelAssemblyName));
            _option = option.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingEntityTypes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void MappingEntityTypes(ModelBuilder modelBuilder)
        {
            if (string.IsNullOrEmpty(_option.ModelAssemblyName))
                return;
            var assembly = Assembly.Load(_option.ModelAssemblyName);
            var types = assembly?.GetTypes();
            var list = types?.Where(t =>
                t.IsClass && !t.IsGenericType && !t.IsAbstract &&
                t.GetInterfaces().Any(m => m.IsAssignableFrom(typeof(BaseModel<>)))).ToList();
            if (list != null && list.Any())
            {
                list.ForEach(t =>
                {
                    if (modelBuilder.Model.FindEntityType(t) == null)
                    {
                        modelBuilder.Model.GetOrAddEntityType(t);
                    }
                });
            }

            }
        public virtual int Add<T>(T entity, bool withTrigger = false) where T : class
        {
            base.Add(entity);
            return withTrigger ? SaveChangesWithTriggers() : SaveChanges();
        }

        public virtual async Task<int> AddAsync<T>(T entity, bool withTrigger = false) where T : class
        {
            await base.AddAsync(entity);
            return await(withTrigger ? SaveChangesWithTriggersAsync() : SaveChangesAsync());
        }

        public virtual int AddRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            base.AddRange(entities);
            return withTrigger ? SaveChangesWithTriggers() : SaveChanges();
        }

        public virtual async Task<int> AddRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            await base.AddRangeAsync(entities);
            return await(withTrigger ? SaveChangesWithTriggersAsync() : SaveChangesAsync());
        }

        public void BulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null) where T : BaseModel<TKey>
        {
            if (!Database.IsSqlServer() && !Database.IsMySql())
                throw new NotSupportedException("This method only supports for SQL Server or MySql.");
        }

        public virtual int Count<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return where==null ? Set<T>().Count() : Set<T>().Count(where);
        }

        public virtual async Task<int> CountAsync<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return await (where == null ? Set<T>().CountAsync() : Set<T>().CountAsync(where));
        }

        public virtual int Delete<T, TKey>(TKey key, bool withTrigger = false) where T : class
        {
            var entity = Find<T>(key);
            Remove(entity);
            return withTrigger ? SaveChangesWithTriggers() : SaveChanges();
        }

        public virtual int Delete<T>(Expression<Func<T, bool>> @where) where T : class
        {
            return Set<T>().Where(@where).Delete();
        }

        public virtual async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> @where) where T : class
        {
            return await Set<T>().Where(@where).DeleteAsync();
        }

        public virtual int Edit<T, TKey>(T entity, bool withTrigger = false) where T : BaseModel<TKey>
        {
            var model = Find<T>(entity.Id);
            Entry(model).CurrentValues.SetValues(entity);
            return withTrigger ? SaveChangesWithTriggers() : SaveChanges();
        }

        public virtual int EditRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            Set<T>().AttachRange(entities.ToArray());
            return withTrigger ? SaveChangesWithTriggers() : SaveChanges();
        }
        public virtual bool Exist<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return @where == null ? Set<T>().Any() : Set<T>().Any(where);
        }

        public virtual async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return await (@where == null ? Set<T>().AnyAsync() : Set<T>().AnyAsync(@where));
        }
        public virtual bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }

        public virtual async Task<bool> EnsureCreatedAsync()
        {
            return await Database.EnsureCreatedAsync();
        }

        public virtual int ExecuteSqlWithNonQuery(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, CancellationToken.None, parameters);
        }

        public virtual async Task<int> ExecuteSqlWithNonQueryAsync(string sql, params object[] parameters)
        {
            return await Database.ExecuteSqlCommandAsync(sql, CancellationToken.None, parameters);
        }

        public virtual IQueryable<T> FilterWithInclude<T>(Func<IQueryable<T>, IQueryable<T>> include, Expression<Func<T, bool>> where) where T : class
        {
            var result = GetDbSet<T>().AsQueryable();
            if (where != null)
            {
                result = GetDbSet<T>().Where(where);
            }
            if (include != null)
            {
                result = include(result);
            }
            return result;
        }

        public virtual T Find<T, TKey>(TKey key) where T : class
        {
            return base.Find<T>(key);
        }

        public virtual async Task<T> FindAsync<T, TKey>(TKey key) where T : class
        {
            return await base.FindAsync<T>(key);
        }

        public virtual IQueryable<T> Get<T>(Expression<Func<T, bool>> where = null, bool asNoTracking = false) where T : class
        {
            var query = Set<T>().AsQueryable();
            if (where != null)
                query = query.Where(where);
            if (asNoTracking)
                query = query.AsNoTracking();
            return query;
        }


        public virtual DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        public virtual T GetSingleOrDefault<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            return where == null ? Set<T>().SingleOrDefault() : Set<T>().SingleOrDefault(where);
        }

        public virtual async Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            return await(where == null ? Set<T>().SingleOrDefaultAsync() : Set<T>().SingleOrDefaultAsync(where));
        }
        public virtual int Update<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class
        {
            if (updateColumns != null && updateColumns.Length > 0)
            {
                if (Entry(model).State == EntityState.Added ||
                    Entry(model).State == EntityState.Detached) Set<T>().Attach(model);

                foreach (var propertyName in updateColumns)
                {
                    Entry(model).Property(propertyName).IsModified = true;
                }
            }
            else
            {
                Entry(model).State = EntityState.Modified;
            }

            return withTrigger ? SaveChangesWithTriggers() : SaveChanges();
        }

        public virtual int Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory) where T : class
        {
            return Set<T>().Where(where).Update(updateFactory);
        }

        public virtual async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory) where T : class
        {
            return await Set<T>().Where(where).UpdateAsync(updateFactory);
        }

        public virtual List<TView> SqlQuery<T, TView>(string sql, params object[] parameters) where T : class
        {
            return Set<T>().FromSql(sql, parameters).Cast<TView>().ToList();
        }

        public virtual Task<List<TView>> SqlQueryAsync<T, TView>(string sql, params object[] parameters)
            where T : class
            where TView : class
        {
            return Set<T>().FromSql(sql, parameters).Cast<TView>().ToListAsync();
        }

        public virtual PaginationResult SqlQueryByPagnation<T, TView>(string sql, string[] orderBys, int pageIndex, int pageSize, Action<TView> eachAction = null)
            where T : class
            where TView : class
        {
            throw new NotImplementedException();
        }

        public int SaveChangesWithTriggers()
        {
            return this.SaveChangesWithTriggers(SaveChanges);
        }

        public int SaveChangesWithTriggers(bool acceptAllChangesOnSuccess)
        {
            return this.SaveChangesWithTriggers(SaveChanges, acceptAllChangesOnSuccess);
        }

        public virtual async Task<int> SaveChangesWithTriggersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.SaveChangesWithTriggersAsync(SaveChangesAsync, acceptAllChangesOnSuccess: true,
           cancellationToken: cancellationToken);
        }

        public async Task<int> SaveChangesWithTriggersAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.SaveChangesWithTriggersAsync(SaveChangesAsync, acceptAllChangesOnSuccess,
                cancellationToken: cancellationToken);
        }
    }
}
