using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace pod_app.DataLayer
{
    public class PodFlowRepository // Implementera IPodRepository
    {
     
        private readonly IMongoCollection<Podcast> podCollection;
        private readonly IMongoClient client;

    public PodFlowRepository()
    {
        
         client = new MongoClient();

        
         var database = client.GetDatabase("OruMongoDB");

      
         podCollection = database.GetCollection<Podcast>("PodProjekt");
    }
        

    }
}



