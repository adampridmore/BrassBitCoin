module BlockChain.Miner

open System
open System.Security.Cryptography
open BlockChain.Types

let computeHash (b: byte[]) = b |> SHA256.Create().ComputeHash

let hash (content : String) = 
    let bytes = content |> System.Text.ASCIIEncoding.UTF8.GetBytes |> computeHash
    BitConverter.ToString( bytes ).Replace("-", "")
let blockHash (block: Block) = 
    [block.index |> string; block.minedBy; block.data; block.previousHash; block.nonce |> string]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hash

let isValidHash (hash:String) = hash.StartsWith("0000")

let isValidBlock (block:BlockWithHash) (lastBlock:BlockWithHash) = 
  let calculatedHash = block.block |> blockHash 

  seq{
    yield calculatedHash = block.hash 
    yield calculatedHash |> isValidHash
    yield lastBlock.hash = block.block.previousHash
    yield (lastBlock.block.index + 1L) = (block.block.index)
  }
  |> Seq.exists(fun valid -> not valid)
  |> not

let newBlock minedBy data (previousBlock: BlockWithHash) = 
    let nonce, hash = 
        Seq.initInfinite (fun i -> i |> int64)
        |> Seq.map(fun nonce -> 
            let block = {
                index = (previousBlock.block.index + 1L)
                minedBy = minedBy
                data = data
                nonce = nonce
                previousHash = previousBlock.hash
            }

            let hash =  block |> blockHash
            nonce,hash)
        |> Seq.where (fun (_, hash) -> isValidHash hash)
        |> Seq.head
            
    let block = {
        index = previousBlock.block.index + 1L;
        minedBy = minedBy;
        data = data;
        nonce = nonce;
        previousHash = previousBlock.hash;
    }

    {
        block = block;
        hash = hash
    }

let genesisBlock =
  { 
    block = 
      {
        index = 0L;
        minedBy = "Genesis"
        data = "Genesis";
        previousHash = "0";
        nonce = 52458L;
      }
    hash = "000021C1766F55BD5D413F0AC128A5D3D6B50E4F0D608B653209C4D468232C11" // block |> blockHash 
  }

let stringReduce seperator (strings: seq<string>) = 
  strings 
  |> Seq.reduce (fun a b -> sprintf "%s%s%s" a seperator b)

let sprintBlock (block: BlockWithHash) = 
  seq{
    yield block.block.index |> string
    yield block.block.minedBy
    yield block.block.data
    yield block.block.previousHash
    yield block.block.nonce |> string
    yield block.hash
  } |> (stringReduce " ")

//let numbeOfBlocksToGenerate = 5

let tuple a = (a, a)

let blockchain numbeOfBlocksToGenerate lastBlock =
  Seq.initInfinite id
  |> Seq.take numbeOfBlocksToGenerate
  |> Seq.map string
  |> Seq.mapFold (fun previousBlock data -> previousBlock |> newBlock "Adam" data |> tuple ) lastBlock
  |> fst
