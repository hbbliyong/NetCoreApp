using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Db
{
    public class MongoBase<T>//: //IMongoBase<T>
    {
        private IMongoCollection<T> mongoClient;
        public MongoBase(MongoClient client)
        {
            var database = client.GetDatabase("test");
        string name     = typeof(T).Name;
            mongoClient = database.GetCollection<T>(name);
        }

        public void InsertOne(T entity)
        {
            ObjectId.GenerateNewId();
            mongoClient.InsertOne(entity);
        }
        public void InsertMany(IEnumerable<T> entitys)
        {
            mongoClient.InsertMany(entitys);
        }

        public T FindFirst(FilterDefinition<T> filter,FindOptions options = null)
        {
            return mongoClient.Find(filter, options).FirstOrDefault();
        }

        public List<T> FindList(FilterDefinition<T> filter, FindOptions option = null)
        {
            return mongoClient.Find(filter, option).ToList();
        }
    }
}
