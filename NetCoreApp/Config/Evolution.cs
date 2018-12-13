using Autofac;
using Autofac.Core;
using NetCoreApp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication1.Respository;

namespace WebApplication1.Config
{
    public class Evolution: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //注入Repository
              builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
              builder.RegisterGeneric(typeof(MongoBase<>)).InstancePerDependency();
            //  builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Repository_"))
            //  .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
    }
}
