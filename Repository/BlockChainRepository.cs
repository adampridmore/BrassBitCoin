using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Repository
{
    public class BlockChainRepository
    {
        private readonly IMongoCollection<Block> _collection;

        public BlockChainRepository(string mongoUrl)
        {
            var url = new MongoDB.Driver.MongoUrl(mongoUrl);
            var client = new MongoDB.Driver.MongoClient(url);
            var database = client.GetDatabase(url.DatabaseName);
            _collection = database.GetCollection<Block>("blockChain");
        }

        public void Save(Block block)
        {
            var collection = _collection;

            collection.InsertOne(block);
        }

        public IList<Block> GetAll()
        {
            return _collection.FindSync(FilterDefinition<Block>.Empty).ToList();
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(FilterDefinition<Block>.Empty);
        }
    }
}
