#load "block.fs"
#load "miner.fs"

open BlockChain.Types
open BlockChain.Miner
let numbeOfBlocksToGenerate = 5

//#time "on"
genesisBlock |> blockchain numbeOfBlocksToGenerate |> Seq.iter (printfn "%A")
