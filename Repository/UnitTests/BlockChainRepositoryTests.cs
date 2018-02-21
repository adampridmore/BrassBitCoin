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

        [Fact]
        public void GetLastBlock()
        {
            var block1 = new Block()
            {
                index = 1
            };

            var block2 = new Block()
            {
                index = 2
            };

            var block3 = new Block()
            {
                index = 3
            };

            repository.Save(block3);
            repository.Save(block1);
            repository.Save(block2);

            var lastBlock = repository.GetLastBlock();

            Assert.Equal(3, lastBlock.index);
        }

        [Fact]
        public void GetCoinOwners()
        {
            repository.Save(new Block() { index = 1, minedBy = "A" });
            repository.Save(new Block() { index = 2, minedBy = "B" });
            repository.Save(new Block() { index = 3, minedBy = "B" });

            var coinOwners = repository.GetCoinOwners();

            Assert.Equal(2, coinOwners.Count);
            Assert.Equal("B", coinOwners[0].Name);
            Assert.Equal(2, coinOwners[0].CoinCount);
            Assert.Equal("A", coinOwners[1].Name);
            Assert.Equal(1, coinOwners[1].CoinCount);
        }
    }
}
