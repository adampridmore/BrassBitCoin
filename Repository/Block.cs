using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Repository
{
    public class Block
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public DateTime createdTimeStampUtc { get; set; }
        public Block()
        {
        }

        public long index { get; set; }
        public string minedBy { get; set; }
        public string data { get; set; }
        public string previousHash { get; set; }
        public long nonce { get; set; }
        public string hash { get; set; }
    }
}