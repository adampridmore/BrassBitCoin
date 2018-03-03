using Repository;
using Xunit;

namespace MyFirstUnitTests
{
    public class BlockDtoRepositoryTests
    {
        private readonly string _url = "mongodb://localhost/BlockChainTest";
        private BlockDtoRepository repository;
        public BlockDtoRepositoryTests()
        {
            repository = new BlockDtoRepository(_url);
            repository.DeleteAll();
        }

        [Fact]
        public void RemoveAllCoins()
        {
            var block = new BlockDto();

            repository.Save(block);
            repository.DeleteAll();

            Assert.Equal(0, repository.GetAll().Count);
        }

        [Fact]
        public void SaveCoin()
        {
            var block = new BlockDto()
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
            var block1 = new BlockDto()
            {
                index = 1
            };

            var block2 = new BlockDto()
            {
                index = 2
            };

            var block3 = new BlockDto()
            {
                index = 3
            };

            repository.Save(block3);
            repository.Save(block1);
            repository.Save(block2);

            var lastBlock = repository.TryGetLastBlock();

            Assert.Equal(3, lastBlock.index);
        }

        [Fact]
        public void GetCoinOwners()
        {
            repository.Save(new BlockDto() { index = 1, minedBy = "A" });
            repository.Save(new BlockDto() { index = 2, minedBy = "B" });
            repository.Save(new BlockDto() { index = 3, minedBy = "B" });

            var minerDtos = repository.GetMinerDtos();

            Assert.Equal(2, minerDtos.Count);
            Assert.Equal("B", minerDtos[0].Name);
            Assert.Equal(2, minerDtos[0].CoinsMined);
            Assert.Equal("A", minerDtos[1].Name);
            Assert.Equal(1, minerDtos[1].CoinsMined);
        }
    }
}
