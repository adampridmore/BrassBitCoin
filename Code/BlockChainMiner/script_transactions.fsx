#load "types.fs"
#load "miner.fs"
#load "transactions.fs"


open BlockChain.Types
open BlockChain.Transaction

let t = {
    from = "Adam"; ``to``= "Dave"; amount = 100
}

t |> transactionToString |> parseTransaction

let miner = {name= "Adam"; balance=10};
let transaction = {
    from = "Dave"; ``to``= "Dave"; amount = 100
}

transaction |> applyTransaction miner

