module BlockChain.Transaction

open BlockChain.Types

let transactionToString (t:Transaction) = 
    sprintf "Transaction,%s,%s,%d" t.from t.``to`` t.amount

let private partsToTransaction a b c blockIndex = 
    let (s, amount) = System.Int32.TryParse(c)

    match s, amount with
    | false, _ -> None
    | true, amount -> 
        Some({
                from = a
                ``to``= b
                amount = amount
                blockIndex=blockIndex
            })


let parseSingleTransaction (txt:string) (blockIndex: int) = 
    let parts = txt.Split([|','|], System.StringSplitOptions.RemoveEmptyEntries)

    match parts.Length, parts.[0] with
    | 4, "Transaction" -> partsToTransaction parts.[1] parts.[2] parts.[3] blockIndex
    | _ -> None

let applyTransaction (miner:Miner) (transaction:Transaction) =
    match transaction.from, transaction.``to`` with
    | f, t when f = miner.name && t = miner.name -> miner
    | _, t when t = miner.name -> {miner with balance = miner.balance + transaction.amount}
    | f, _ when f = miner.name -> {miner with balance = miner.balance - transaction.amount}
    | _,_ -> miner


let applyTransactionToMiners (miners: seq<Miner>) (transaction: Transaction) =
    let addMissingMiner name (miners: seq<Miner>)=
        match miners |> Seq.exists(fun m -> m.name = name) with
        | false-> Seq.concat [miners; Seq.singleton {name = name; balance = 0; coinsMined = 0}]
        | true -> miners

    miners
    |> addMissingMiner (transaction.``from``)
    |> addMissingMiner (transaction.``to`` )
    |> Seq.map(fun miner -> transaction |> applyTransaction miner)

let getAllTransations(repository: Repository.BlockDtoRepository) = 
    repository.GetTransactions() 
    |> Seq.map (DtoHelpers.transactionDtoToTransaction)
 
let getAllMiners(repository: Repository.BlockDtoRepository ) =
    let miners = 
        repository.GetMinerDtos() 
        |> Seq.map DtoHelpers.minerDtoToMiner

    getAllTransations(repository)
    |> Seq.fold(fun minersState transaction -> 
        let newMiners = applyTransactionToMiners minersState transaction
        newMiners
    ) miners
    
    
