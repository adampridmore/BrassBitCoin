using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Repository.Dto
{
    public class BlockDto
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public DateTime? createdTimeStampUtc { get; set; }
        public BlockDto()
        {
        }

        public BlockDto(int index,
            string minedBy,
            string data,
            int nonce,
            string previousHash,
            string hash)
        {
            this.index = index;
            this.minedBy = minedBy;
            this.data = data;
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