module BlockChain.Transaction

open BlockChain.Types

let transactionToString (t:Transaction) = 
    sprintf "Transaction,%s,%s,%d" t.from t.``to`` t.ammount

let private partsToTransaction a b c = 
    let (s, ammount) = System.Int32.TryParse(c)

    match s, ammount with
    | false, _ -> None
    | true, ammount -> Some({from = a; ``to``= b; ammount = ammount})


let parseSingleTransaction (txt:string) = 
    let parts = txt.Split([|','|], System.StringSplitOptions.RemoveEmptyEntries)

    match parts.Length, parts.[0] with
    | 4, "Transaction" -> partsToTransaction parts.[1] parts.[2] parts.[3]
    | _ -> None

let applyTransaction (miner:Miner) (transaction:Transaction) =
    match transaction.from, transaction.``to`` with
    | f, t when f = miner.name && t = miner.name -> miner
    | _, t when t = miner.name -> {miner with balance = miner.balance + transaction.ammount}
    | f, _ when f = miner.name -> {miner with balance = miner.balance - transaction.ammount}
    | _,_ -> miner


let applyTransactionToMiners (miners: seq<Miner>) (transaction: Transaction) =
    let addMissingMiner name (miners: seq<Miner>)=
        match miners |> Seq.exists(fun m -> m.name = name) with
        | false-> Seq.concat [miners; Seq.singleton {name = name; balance = 0}]
        | true -> miners

    miners
    |> addMissingMiner (transaction.``from``)
    |> addMissingMiner (transaction.``to`` )
    |> Seq.map(fun miner -> transaction |> applyTransaction miner)