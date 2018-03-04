#load "types.fs"
#load "minerHelpers.fs"
#load "miner.fs"
#load "transactions.fs"

open BlockChain.Types
open BlockChain.Miner
open BlockChain.Transaction

let miners = 
  [
    {
      name = "Adam";
      balance = 100
    };
    {
      name = "Dave";
      balance = 200
    }
  ]

let transaction = 
  {
    from ="Betty";
    ``to`` = "Bob";
    amount = 10
  }

let addMissingMiner name (miners: seq<Miner>)=
    match miners |> Seq.exists(fun m -> m.name = name) with
    | false-> Seq.concat [miners; Seq.singleton {name = name; balance = 0}]
    | true -> miners

let applyTransactionToMiners (miners: seq<Miner>) (transaction: Transaction) =
    miners
    |> addMissingMiner (transaction.``from``)
    |> addMissingMiner (transaction.``to`` )
    |> Seq.map(fun miner -> transaction |> applyTransaction miner)

applyTransactionToMiners miners transaction
