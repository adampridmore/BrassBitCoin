module BlockChain.minerTests
open Xunit
open BlockChain.Types
open BlockChain.Miner
open BlockChain

let createNewBlock() =     
  { 
    block = 
      {
        index = 1;
        minedBy = "TestMinerName"
        data = "TestData";
        previousHash = genesisBlock.hash;
        nonce = 113095;
      }
    hash = "0000C12840C025A31B0F68348382630BBD7DDD9FCDDC9431C8903DF76F22FDC5"
  }

[<Fact>]
let ``Genesis block``() =
  Assert.Equal(0, genesisBlock.block.index)
  Assert.Equal("Genesis", genesisBlock.block.data)
  Assert.Equal("Genesis", genesisBlock.block.minedBy)
  Assert.Equal(52458, genesisBlock.block.nonce)
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
  Assert.Equal(1, newBlock.block.index)
  Assert.Equal(genesisBlock.hash, newBlock.block.previousHash)
  Assert.Equal(113095, newBlock.block.nonce)

[<Fact>]
let ``hash test``()=
  Assert.Equal("A7FD4C665FBF6375D99046EF9C525E8578FEB7A4794D119447282DB151C12CAE", ("Some Text" |> hashString))

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
  match (isValidBlock (createNewBlock()) genesisBlock) with
  | Valid -> ()
  | Invalid(errors) -> failwith (errors |> Seq.reduce (sprintf "%s %s"))

let assertInvalidBlock expectedErrorMessage block = 
  let isValid = isValidBlock block genesisBlock
  match isValid with
  | Valid -> failwith("Should not be valid")
  | Invalid(errors) -> Assert.Equal(expectedErrorMessage, errors |> Seq.find(fun error->error = expectedErrorMessage))
  
[<Fact>]
let ``is not a valid block - hash isn't hash of block``()=
  let notAValidBlock = {createNewBlock() with hash = "00001234"}

  notAValidBlock |> assertInvalidBlock "Hash does not match hash of block."
  
[<Fact>]
let ``is not a valid block - invalid hash``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with hash = "1234"}

  notAValidBlock |> assertInvalidBlock "Hash does not meet rules."

[<Fact>]
let ``is not a valid block - invalid minerName``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with block = {validBlock.block with minedBy = "Not Valid"}}
  notAValidBlock |> assertInvalidBlock "MinedBy is invalid, must be alpha-numeric only."


[<Fact>]
let ``is not a valid block - block parent hash is icorret``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with block = {validBlock.block with previousHash= "00001234"}}
  notAValidBlock |> assertInvalidBlock "Previous hash does not match."

[<Fact>]
let ``is not a valid block - incorrect index``() =
  let validBlock = createNewBlock()
  let notAValidBlock = {validBlock with block = {validBlock.block with index = 99}}
  notAValidBlock |> assertInvalidBlock "Invalid index."
