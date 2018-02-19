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
    public class Class1
    {
        [Fact]
        public void SaveCoin()
        {
            var block = new Block()
            {
                index = 1
            };

            var repository = new BlockChainRepository();
            repository.Save(block);

            var allBlocks = repository.GetAll();

            Assert.Equal(1, allBlocks.Count);
        }
    }
}
