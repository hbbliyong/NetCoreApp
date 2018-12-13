using MongoDB.Driver;
using NetCoreApp.Db;
using NetCoreApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    //public class UserInfoService : MongoBase<UserInfo>, IUserInfoService
   public class UserInfoService :IUserInfoService
    {
        private MongoBase<UserInfo> mongoBase;
        public UserInfoService(MongoBase<UserInfo> client)
        {
            mongoBase = client;
        }

        public UserInfo FindFirst(FilterDefinition<UserInfo> filter)
        {
            return   mongoBase.FindFirst(filter);
        }

        public void InsertOne(UserInfo userInfo)
        {
             mongoBase.InsertOne(entity: userInfo);
        }
    }
}
