module BlockChain.BlockHelpers
open BlockChain.Types
open Repository

let DtoToBlock (lastBlockDto:Repository.Block ) =
    {
        block = {
                index = lastBlockDto.index;
                minedBy = lastBlockDto.minedBy;
                data = lastBlockDto.data;
                previousHash = lastBlockDto.previousHash
                nonce = lastBlockDto.nonce
            }
        hash = lastBlockDto.hash
    }

let BlockToDto(blockWithHash:BlockWithHash) =
    new Repository.Block(
        blockWithHash.block.index,
        blockWithHash.block.data,
        blockWithHash.block.minedBy,
        blockWithHash.block.nonce,
        blockWithHash.block.previousHash,
        blockWithHash.hash);
