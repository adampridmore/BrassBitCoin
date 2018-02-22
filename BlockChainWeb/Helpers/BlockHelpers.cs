using BlockChain;
using static BlockChain.Types;

namespace BlockChainWeb.Helpers
{
    public class BlockHelpers
    {
        public static Types.BlockWithHash DtoToBlock(Repository.Block lastBlockDto)
        {
            var block = new Types.Block(lastBlockDto.index, lastBlockDto.minedBy, lastBlockDto.data, lastBlockDto.previousHash, lastBlockDto.nonce);
            return new Types.BlockWithHash(block, lastBlockDto.hash);
        }

        public static Repository.Block BlockToDto(BlockWithHash blockWithHash)
        {
            return new Repository.Block()
            {
                index = blockWithHash.block.index,
                data = blockWithHash.block.data,
                minedBy = blockWithHash.block.minedBy,
                nonce = blockWithHash.block.nonce,
                previousHash = blockWithHash.block.previousHash,
                hash = blockWithHash.hash
            };
        }
    }
}
