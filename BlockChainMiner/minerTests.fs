module minerTests
open Xunit
open BlockChain.Miner
open BlockChain

[<Fact>]
let ``Genesis block``() =
  Assert.Equal(0L, genesisBlock.block.index)
  Assert.Equal("Genesis", genesisBlock.block.data)
  Assert.Equal("Genesis", genesisBlock.block.minedBy)
  Assert.Equal(52458L, genesisBlock.block.nonce)
  Assert.Equal("0", genesisBlock.block.previousHash)
  Assert.Equal("000021C1766F55BD5D413F0AC128A5D3D6B50E4F0D608B653209C4D468232C11", genesisBlock.hash)
   
[<Fact>]
let ``is valid hash``()=
  Assert.True(genesisBlock.hash |> isValidHash)

[<Fact>]
let ``mine block``()=
  let newBlock = genesisBlock |> Miner.newBlock "TestMinerName" "TestData"
  Assert.Equal("TestMinerName", newBlock.block.minedBy)
  Assert.Equal("TestData", newBlock.block.data)
  Assert.Equal(1L, newBlock.block.index)
  Assert.Equal(genesisBlock.hash, newBlock.block.previousHash)
  Assert.Equal(113095L, newBlock.block.nonce)



