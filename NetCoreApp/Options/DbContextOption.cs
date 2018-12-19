using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Options
{
    public class DbContextOption
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 实体程序集名称
        /// </summary>
        public string ModelAssemblyName { get; set; }
    }
}
