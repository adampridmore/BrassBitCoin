module BlockChain.MinerHelpers
open BlockChain.Types

let stringReduce seperator (strings: seq<string>) = strings |> Seq.reduce (fun a b -> sprintf "%s%s%s" a seperator b)

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