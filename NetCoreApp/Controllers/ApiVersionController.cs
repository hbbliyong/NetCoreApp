using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ApiVersionController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Value1 from Version 1", "value2 from Version 1" };
        }
        [HttpGet,MapToApiVersion("2.0")]

        public IEnumerable<string> GetV2()
        {
            return new string[] { "Value1 from Version 2", "value2 from Version 2" };
        }
    }

    [ApiVersion("2.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    public class ApiVersionV2Controller : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Value1 from Version 1", "value2 from Version 1" };
        }
    }
}