using Repository;
using Xunit;

namespace MyFirstUnitTests
{
    //     type Block = {
    //     index :int64
    //     minedBy: string
    //     data :  string
    //     previousHash : string
    //     nonce: int64
    // }
    public class BlockChainRepositoryTests
    {
        private readonly string _url = "mongodb://localhost/BlockChainTest";
        private BlockChainRepository repository;
        public BlockChainRepositoryTests()
        {
            repository = new BlockChainRepository(_url);
            repository.DeleteAll();
        }

        [Fact]
        public void RemoveAllCoins()
        {
            var block = new Block();

            repository.Save(block);
            repository.DeleteAll();

            Assert.Equal(0, repository.GetAll().Count);
        }

        [Fact]
        public void SaveCoin()
        {
            var block = new Block()
            {
                index = 10
            };

            repository.Save(block);

            var allBlocks = repository.GetAll();

            Assert.Equal(1, allBlocks.Count);
            Assert.Equal(10, allBlocks[0].index);
        }
    }
}
