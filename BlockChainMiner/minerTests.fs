module minerTests
open Xunit
open BlockChain.Miner
open BlockChain

let createNewBlock() = genesisBlock |> Miner.newBlock "TestMinerName" "TestData"

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
  let newBlock = createNewBlock()

  Assert.Equal("TestMinerName", newBlock.block.minedBy)
  Assert.Equal("TestData", newBlock.block.data)
  Assert.Equal(1L, newBlock.block.index)
  Assert.Equal(genesisBlock.hash, newBlock.block.previousHash)
  Assert.Equal(113095L, newBlock.block.nonce)

[<Fact>]
let ``hash test``()=
  Assert.Equal("A7FD4C665FBF6375D99046EF9C525E8578FEB7A4794D119447282DB151C12CAE", ("Some Text" |> hash))

[<Fact>]
let ``block hash``()=
  Assert.Equal(genesisBlock.hash, (genesisBlock.block |> blockHash))

[<Fact>]
let ``is a valid hash``=
  Assert.True("0000123" |> isValidHash)

[<Fact>]
let ``is not a valid hash``=
  Assert.True("1234" |> isValidHash)

[<Fact>]
let ``is a valid block``()=
  genesisBlock |> isValidBlock

[<Fact>]
let ``is not a valid block - hash isn't hash of block``()=
  let notAValidBlock = {createNewBlock() with hash = "00001234"}
  Assert.False(isValidBlock notAValidBlock genesisBlock)

[<Fact>]
let ``is not a valid block - invalid hash``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with block = {validBlock.block with nonce = 1L}}
  Assert.False(isValidBlock notAValidBlock genesisBlock)

[<Fact>]
let ``is not a valid block - block parent hash is icorret``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with block = {validBlock.block with previousHash= "00001234"}}
  Assert.False(isValidBlock notAValidBlock genesisBlock)

[<Fact>]
let ``is not a valid block - incorrect index``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with block = {validBlock.block with index = 99L}}
  Assert.False(isValidBlock notAValidBlock genesisBlock)
