module BlockChain.MinerHelpers
open BlockChain.Types
open System

let parseBlock (blockText:string) = 
   let parts = blockText.Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries)

   {
        block = 
            {
                index= Int32.Parse(parts.[0])
                minedBy=parts.[1]
                data= parts.[2]
                previousHash= parts.[3]
                nonce = Int32.Parse(parts.[4])
            }
        hash = parts.[5]
    }

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


let tuple a = (a, a)