module BlockChain.Miner

open System
open System.Security.Cryptography
open BlockChain.Types
open BlockChain.MinerHelpers

let computeHash (b: byte[]) = b |> SHA256.Create().ComputeHash

let hashString (content : String) = 
    let bytes = content |> System.Text.ASCIIEncoding.UTF8.GetBytes |> computeHash
    BitConverter.ToString( bytes ).Replace("-", "")

let blockHash (block: Block) = 
    [block.index |> string; block.minedBy; block.data; block.previousHash; block.nonce |> string]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hashString

let public isValidHash (hash:String) = hash.StartsWith("0000")

let private minedByRegularExpression = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]*$");

let isValidMinedBy minedBy = 
    minedByRegularExpression.IsMatch(minedBy)

let isValidBlock (block:BlockWithHash) (lastBlock:BlockWithHash) = 
  let validation =
    seq{
      yield block.block.minedBy |> isValidMinedBy, "MinedBy is invalid, must be alpha-numeric only."
      yield block.block |> blockHash = block.hash, "Hash does not match hash of block." 
      yield (block.hash |> isValidHash), "Hash does not meet rules."
      yield (lastBlock.hash = block.block.previousHash), "Previous hash does not match."
      yield (lastBlock.block.index + 1) = (block.block.index), "Invalid index."
    }
    |> Seq.where(fun (valid , _) -> not valid)
    |> Seq.toList
    
  match (validation |> List.isEmpty ) with
  | true -> Valid
  | false -> Invalid(validation |> Seq.map(snd))

let newBlock minedBy data (previousBlock: BlockWithHash) = 
    Seq.initInfinite id
    |> Seq.map(fun nonce -> 
        let block = {
            index = (previousBlock.block.index + 1)
            minedBy = minedBy
            data = data
            nonce = nonce
            previousHash = previousBlock.hash
        }
        {
            block = block;
            hash = block |> blockHash
        }
    )
    |> Seq.find (fun (blockWithHash) -> blockWithHash.hash |> isValidHash)

let genesisBlock =
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

let blockchain numbeOfBlocksToGenerate minedBy lastBlock =
  Seq.initInfinite id
  |> Seq.take numbeOfBlocksToGenerate
  |> Seq.map string
  |> Seq.mapFold (fun previousBlock data -> previousBlock |> newBlock minedBy data |> tuple ) lastBlock
  |> fst
