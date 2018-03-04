using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using Repository.Dto;

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

        public IList<MinerDto> GetMinerDtos()
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
                    g => new MinerDto (g.Key, g.Sum(key => 1))
                )
                .SortByDescending(co => co.CoinsMined)
                .ToList();
        }

        public IList<TransactionDto> GetTransactions()
        {
            var blocks = GetAll();
            return BlockToTansactionDtos(blocks).ToList();

        }

        private IEnumerable<TransactionDto> BlockToTansactionDtos(IList<BlockDto> blocks)
        {
            return blocks
                .SelectMany(b => BlockToTansactionDtos(b.data));
        }

        private IEnumerable<TransactionDto> BlockToTansactionDtos(string data)
        {
            return data
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(line => TryParseLine(line))
                .Where(transaction => transaction != null);
        }
        public static TransactionDto TryParseLine(string line)
        {
            if (!line.StartsWith("Transaction"))
            {
                return null;
            }

            var lines = line.Split(new[] { "," }, StringSplitOptions.None);
            if (lines.Length != 4)
            {
                return null;
            }

            if (!int.TryParse(lines[3], out int ammount))
            {
                return null;
            }

            return new TransactionDto(lines[1], lines[2], ammount);
        }
    }
}
