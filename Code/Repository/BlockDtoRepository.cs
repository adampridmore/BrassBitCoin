using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;

namespace Repository
{
    public class BlockDtoRepository
    {
        private readonly IMongoCollection<BlockDto> _collection;

        public BlockDtoRepository(string mongoUrl)
        {
            var url = new MongoDB.Driver.MongoUrl(mongoUrl);
            var client = new MongoDB.Driver.MongoClient(url);
            var database = client.GetDatabase(url.DatabaseName);
            _collection = database.GetCollection<BlockDto>("blockChain");
        }

        public void Save(BlockDto block)
        {
            var collection = _collection;

            if (block.createdTimeStampUtc == null)
            {
                block.createdTimeStampUtc = DateTime.UtcNow;
            }

            collection.InsertOne(block);
        }

        public IList<BlockDto> GetAll()
        {
            return _collection.FindSync(FilterDefinition<BlockDto>.Empty).ToList();
        }

        public BlockDto TryGetLastBlock()
        {
            return _collection.AsQueryable()
                .OrderByDescending(b => b.index)
                .FirstOrDefault();
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(FilterDefinition<BlockDto>.Empty);
        }

        public IList<CoinOwner> GetCoinOwners()
        {
            /*
            db.blockChain.aggregate([{
                $group:{
                    _id: "$minedBy",
                    count : { $sum : 1}
                }
            },{
                $sort:{
                    count: -1
                }
            }]) */

            return _collection
                .Aggregate()
                .Group(key => key.minedBy,
                    g => new CoinOwner { Name = g.Key, CoinsMined = g.Sum(key => 1) }
                )
                .SortByDescending(co => co.CoinsMined)
                .ToList();
        }
    }
}
