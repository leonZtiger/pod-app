using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; }
        public MongoDbContext(string connection_str,string db_name)
        {
            Database = new MongoClient(connection_str).GetDatabase(db_name);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
                => Database.GetCollection<T>(name);
    }
}
