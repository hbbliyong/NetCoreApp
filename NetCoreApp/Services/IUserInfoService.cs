using MongoDB.Driver;
using NetCoreApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IUserInfoService
    {
        UserInfo FindFirst(FilterDefinition<UserInfo> filter);
        void InsertOne(UserInfo userInfo);
    }
}
