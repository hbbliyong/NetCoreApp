
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.model;

namespace WebApplication1
{
    public class Md5Context:DbContext
    {
        //方式1，直接重写基类的配置方法
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("server=192.168.99.100;port=3306;database=test;uid=root;pwd=root;");
        //}
        //方法2，写到配置文件，在startup里面进行注入
        public Md5Context(DbContextOptions options):base(options)
        {

        }
        public virtual DbSet<Test> Tests { get; set; }
    }
}
