#I @".\bin\Debug\netstandard2.0"
#r @"Repository.dll"
#r @"BlockChain.dll"

open BlockChain.Miner
open BlockChain.MinerHelpers
open BlockChain

let data = 
  //  [   
        "Transaction,Dave,Fred,5"
  //  ]
//    |> stringReduce System.Environment.NewLine

"11 Dave Transaction,Adam,Dave,5 0000D7B3DB4778929CAB77A32B2C11D9A92A59FF70245C519EC21733EB1799BE 20436 0000439FB6C3954F5E4E77D611BFC566EE964538BAA38F6604C805F1BC7C032F"
|> parseBlock
|> Miner.newBlock "Dave" data
|> sprintBlock
|> printf "%s"

//let miners = (Seq.empty<BlockChain.Types.Miner>)

//data 
//|> Transaction.parseSingleTransaction
//|> Option.get
//|> Transaction.applyTransactionToMiners miners
