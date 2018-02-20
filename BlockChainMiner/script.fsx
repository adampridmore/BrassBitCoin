#load "block.fs"
#load "miner.fs"

open Block
open Miner
let numbeOfBlocksToGenerate = 5

//#time "on"
genesisBlock |> blockchain numbeOfBlocksToGenerate |> Seq.iter (printfn "%A")
