using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Repository
{
    public class BlockChainRepository
    {
        public void Save(Block block)
        {
            MongoDB.Driver.IMongoCollection<Block> collection = GetCollection();

            collection.InsertOne(block);
        }
        
        internal IList<Block> GetAll()
        {
            return GetCollection().FindSync(FilterDefinition<Block>.Empty).ToList();
        }

        internal void DeleteAll()
        {
            GetCollection().DeleteMany(FilterDefinition<Block>.Empty);
        }
        
        private static MongoDB.Driver.IMongoCollection<Block> GetCollection()
        {
            var url = new MongoDB.Driver.MongoUrl("mongodb://localhost/BlockChain");
            var client = new MongoDB.Driver.MongoClient(url);
            var database = client.GetDatabase(url.DatabaseName);
            var collection = database.GetCollection<Block>("blockChain");
            return collection;
        }
    }
}
