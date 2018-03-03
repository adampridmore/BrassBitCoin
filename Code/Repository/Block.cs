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

        public DateTime? createdTimeStampUtc { get; set; }
        public Block()
        {
        }

        public Block(int index,
            string data,
            string minedBy,
            int nonce,
            string previousHash,
            string hash)
        {
            this.index = index;
            this.data = data;
            this.minedBy = minedBy;
            this.nonce = nonce;
            this.previousHash = previousHash;
            this.hash = hash;
        }
    
    public int index { get; set; }
    public string minedBy { get; set; }
    public string data { get; set; }
    public string previousHash { get; set; }
    public int nonce { get; set; }
    public string hash { get; set; }
}
}