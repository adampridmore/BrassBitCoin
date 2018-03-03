module BlockHelpersTests

open Xunit
open BlockChain.Types
open BlockChain

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

    let blockDto = blockWithHash |> BlockHelpers.BlockToDto
    Assert.Equal(blockWithHash.block.index, blockDto.index)
    Assert.Equal(blockWithHash.block.minedBy, blockDto.minedBy)
    Assert.Equal(blockWithHash.block.data, blockDto.data)
    Assert.Equal(blockWithHash.block.previousHash, blockDto.previousHash)
    Assert.Equal(blockWithHash.block.nonce, blockDto.nonce)
    Assert.Equal(blockWithHash.hash, blockDto.hash)


[<Fact>]
let ``Dto to block``() =
    let blockDto = new Repository.Block(1,"myMinedBy", "myData", 123,"myPreviousHash",  "myHash");

    let blockWithHash = blockDto |> BlockHelpers.DtoToBlock

    Assert.Equal(1, blockWithHash.block.index)
    Assert.Equal("myMinedBy", blockWithHash.block.minedBy)
    Assert.Equal("myData", blockWithHash.block.data)
    Assert.Equal(123, blockWithHash.block.nonce)
    Assert.Equal("myPreviousHash", blockWithHash.block.previousHash)
    Assert.Equal("myHash", blockWithHash.hash)
