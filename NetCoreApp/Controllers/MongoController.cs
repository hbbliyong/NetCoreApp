using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreApp.Model;
using WebApplication1.Services;

namespace NetCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoController : ControllerBase
    {
        private IUserInfoService userInfoService;
        public MongoController(IUserInfoService _userInfoService)
        {
            this.userInfoService = _userInfoService;
        }
        [HttpGet]
        public ActionResult<UserInfo> Index()
        {
            UserInfo user = userInfoService.FindFirst(Builders<UserInfo>.Filter.Eq(p => p.Name, "Test"));

            userInfoService.InsertOne(new UserInfo() { Name = "Test", Id = 222 });

            user = userInfoService.FindFirst(Builders<UserInfo>.Filter.Eq(p => p.Name, "Test"));
            return user;
        }
    }
}