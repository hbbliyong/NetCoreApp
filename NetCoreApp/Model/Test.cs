using NetCoreApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Model
{
    [Table("test")]
    public class Test:BaseModel<int>
    {
        private int id;
        [Key]
        public override int Id { get { return id; } set { id = value; } }
       
        public string Name { get; set; }
    }
}
