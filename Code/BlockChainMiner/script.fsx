#load "block.fs"
#load "miner.fs"

open BlockChain.Types
open BlockChain.Miner

let numbeOfBlocksToGenerate = 5

//#time "on"

// genesisBlock |> blockchain numbeOfBlocksToGenerate |> Seq.iter (printfn "%A")

let previousBlockWithHash = {
  block = {
    Block.index = 9;
    minedBy = "Adam";
    data = "0";
    previousHash = "0000D6CBE5B197E5F0ED88F773A6BD03F1A5D6E39A7C75E297DC1D12685BE5C8";
    nonce = 6334;
  };
  hash = "0000DEA9218EB34470BAD8C08D8BC1FD73D304739FD55E8652F17A3D3CD3876F"
}

previousBlockWithHash |> blockchain 1 "Adam" |> Seq.iter (printfn "%A")



genesisBlock |> sprintBlock

