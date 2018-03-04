module BlockChain.DtoHelpersTests

open Xunit
open BlockChain.Types
open BlockChain
open BlockChain.DtoHelpers
open Repository.Dto

[<Fact>]
let ``To block dto``() =
    let blockWithHash =
      { 
        block = 
          {
            index = 0;
            minedBy = "Genesis"
            data = "Genesis";
            previousHash = "0";
            nonce = 52458;
          }
        hash = "000021C1766F55BD5D413F0AC128A5D3D6B50E4F0D608B653209C4D468232C11" // block |> blockHash 
    }

    let blockDto = blockWithHash |> BlockToDto
    Assert.Equal(blockWithHash.block.index, blockDto.index)
    Assert.Equal(blockWithHash.block.minedBy, blockDto.minedBy)
    Assert.Equal(blockWithHash.block.data, blockDto.data)
    Assert.Equal(blockWithHash.block.previousHash, blockDto.previousHash)
    Assert.Equal(blockWithHash.block.nonce, blockDto.nonce)
    Assert.Equal(blockWithHash.hash, blockDto.hash)


[<Fact>]
let ``Dto to block``() =
    let blockDto = new BlockDto(1,"myMinedBy", "myData", 123,"myPreviousHash",  "myHash");

    let blockWithHash = blockDto |> DtoToBlock

    Assert.Equal(1, blockWithHash.block.index)
    Assert.Equal("myMinedBy", blockWithHash.block.minedBy)
    Assert.Equal("myData", blockWithHash.block.data)
    Assert.Equal(123, blockWithHash.block.nonce)
    Assert.Equal("myPreviousHash", blockWithHash.block.previousHash)
    Assert.Equal("myHash", blockWithHash.hash)


[<Fact>]
let ``Dto to Miner``()=
    let miner = 
        new Repository.Dto.MinerDto("myName",10) 
        |> minerDtoToMiner

    Assert.Equal("myName", miner.name); 
    Assert.Equal(10, miner.coinsMined); 
    Assert.Equal(10, miner.balance); 

[<Fact>]
let ``Dto to transaction``()= 
    let transaction = 
        new TransactionDto("myFrom", "myTo", 123, 100)
        |> transactionDtoToTransaction

    Assert.Equal("myFrom", transaction.from)
    Assert.Equal("myTo", transaction.``to``)
    Assert.Equal(123, transaction.amount)
    Assert.Equal(100, transaction.blockIndex)

    
