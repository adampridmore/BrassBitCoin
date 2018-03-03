module BlockChain.DtoHelpers
open BlockChain.Types
open Repository.Dto
open System

let DtoToBlock (lastBlockDto:BlockDto ) =
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
    new BlockDto(
        blockWithHash.block.index,
        blockWithHash.block.minedBy,
        blockWithHash.block.data,
        blockWithHash.block.nonce,
        blockWithHash.block.previousHash,
        blockWithHash.hash);

