using Repository.Dto;
using System.Collections.Generic;
using Xunit;

namespace Repository.UnitTests
{
    public class BlockDtoRepositoryTests
    {
        public static BlockDtoRepository CreateRepositoryForTestDb()
        {
            string _url = "mongodb://localhost/BlockChainTest";
            return new BlockDtoRepository(_url);
        }
                
        private BlockDtoRepository repository;
        public BlockDtoRepositoryTests()
        {
            repository = CreateRepositoryForTestDb();
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

        [Fact]
        public void GetTransactions()
        {
            var transactionLines = new List<string>{
                "Transaction,Adam,Dave, 1",
                "Transaction,Adam,Dave, 2"
            };

            var transactionText = string.Join("\n", transactionLines);

            repository.Save(new BlockDto {
                index = 1,
                data = transactionText,
                minedBy = "Adam" }
            );

            var transactions = repository.GetTransactions();

            Assert.Equal(2, transactions.Count);
            Assert.Equal("Adam", transactions[0].from);
            Assert.Equal("Dave", transactions[0].to);
            Assert.Equal(1, transactions[0].blockIndex);
            Assert.Equal(1, transactions[0].amount);

        }

        [Fact]
        public void TryParseInvalidLine()
        {
            var transaction = BlockDtoRepository.TryParseLine("InvalidTransaction", new BlockDto());
            Assert.Null(transaction);
        }

        [Fact]
        public void TryParseLine()
        {
            var transaction = BlockDtoRepository.TryParseLine("Transaction,Adam,Dave,123", new BlockDto());
            Assert.NotNull(transaction);

            Assert.Equal("Adam", transaction.from);
            Assert.Equal("Dave", transaction.to);
            Assert.Equal(123, transaction.amount);
        }

        [Fact]
        public void TryParseInvalidLine_wrong_number_of_fields()
        {
            var transaction = BlockDtoRepository.TryParseLine("Transaction,Adam,Dave,123,", new BlockDto());
            Assert.Null(transaction);
        }


        [Fact]
        public void TryParseInvalidLine_invalid_amount()
        {
            var transaction = BlockDtoRepository.TryParseLine("Transaction,Adam,Dave,1.2", new BlockDto());
            Assert.Null(transaction);
        }

        [Fact]
        public void GetAll()
        {
            repository.Save(new BlockDto() { index = 20, minedBy = "B" });
            repository.Save(new BlockDto() { index = 10, minedBy = "A" });

            var allBlocks = repository.GetAll();
            Assert.Equal(2, allBlocks.Count);

            Assert.Equal(10, allBlocks[0].index);
            Assert.Equal(20,allBlocks[1].index);
        }
    }
}
