using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ITestService md5Context;
        IUserService userService;
        public ValuesController(ITestService md5Context, IUserService userService)
        {
            this.md5Context = md5Context;
            this.userService = userService;
        }
        //public ValuesController( IUserService userService)
        //{
        //   // this.md5Context = md5Context;
        //    this.userService = userService;
        //}
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //Md5Context md5Context = new Md5Context();
            List<Test> list = md5Context.GetTests().Result;
            return list.Select(t => t.Name).ToList();
            // return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            List<User> list = userService.GetUsers().Result;
            return string.Join(",", list.Select(t => t.UserName+"==="+t.Password).ToArray<string>());
           // return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
