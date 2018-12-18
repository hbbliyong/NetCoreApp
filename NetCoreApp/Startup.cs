using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using WebApplication1.Config;
using WebApplication1.Respository;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Md5Context>(options => options.UseMySql(Configuration.GetConnectionString("MySql")));

            //配置mongodb依赖
            string mongoConnectionString = Configuration.GetSection("Mongo:ConnectionStrings").Value;
            services.AddSingleton(new MongoClient(mongoConnectionString));
            // services.AddTransient<ITestService, TestService>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Swagger

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = "NetCoreApp", Version = "v1" });

                //Set the comments path for the Swagger Json and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            #endregion

            #region Api版本控制服务
            services.AddApiVersioning(o =>
            {
                //ReportApiVersion 属性是一个布尔类型，如果设置为true, 在Api请求的响应头部，会追加当前Api支持的版本
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            #endregion
            #region autofac

            var builder = new ContainerBuilder();
            builder.RegisterModule(new Evolution());
            builder.Populate(services);
            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApp V1");
            });

            app.UseMvc();
        }
    }
}
