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

let minerDtoToMiner (minerDto:Repository.Dto.MinerDto) = 
    {
        name = minerDto.Name;
        coinsMined = minerDto.CoinsMined;
        balance = minerDto.CoinsMined
    }

let transactionDtoToTransaction (dto: TransactionDto) =
    {
        from = dto.from;
        ``to`` = dto.``to``;
        amount = dto.amount;
        blockIndex = dto.blockIndex
    }

