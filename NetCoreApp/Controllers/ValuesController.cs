using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Model;
using WebApplication1.Services;

namespace NetCoreApp.Controllers
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
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
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
            return string.Join(",", list.Select(t => t.UserName + "===" + t.Password).ToArray<string>());
            // return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        /// <summary>
        /// 接收Userinfo实体
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="value">用户信息</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public string PostData(string id,[FromBody] UserInfo value)
        {
            return $"{id}=={value.ToString()}";
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
