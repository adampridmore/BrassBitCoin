using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Repository
{
    public class Block
    {
        [BsonId(IdGenerator=typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public Block()
        {
        }

        public int Index { get; set; }
    }
}