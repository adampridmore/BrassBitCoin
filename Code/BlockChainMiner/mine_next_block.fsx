#load "types.fs"
#load "minerHelpers.fs"
#load "miner.fs"
#load "transactions.fs"

open BlockChain.Miner
open BlockChain.MinerHelpers
open BlockChain
open Transaction

let data = 
  //  [   
        "Transaction,Adam,Dave,5"
  //  ]
//    |> stringReduce System.Environment.NewLine

"10 Dave 0 000066FFAD8BC368DACDF4A17EFED490BC36B60A4B7A2E522DCE0A3179F6DC5D 38546 0000D7B3DB4778929CAB77A32B2C11D9A92A59FF70245C519EC21733EB1799BE"
|> parseBlock
|> Miner.newBlock "Dave" data
|> sprintBlock

let miners = (Seq.empty<BlockChain.Types.Miner>)

data 
|> Transaction.parseSingleTransaction
|> Option.get
|> Transaction.applyTransactionToMiners miners
