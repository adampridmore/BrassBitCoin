module BlockChain.transactionTests
open Xunit
open BlockChain.Types
open BlockChain.Transaction
open Repository.Dto

[<Fact>]
let ``transaction to string``() =
    let txt =
        { from = "Adam"; ``to``= "Dave"; amount = 100 ; blockIndex=1}
        |> transactionToString 
    
    Assert.Equal("Transaction,Adam,Dave,100", txt)

[<Fact>]
let ``parse transaction``() =
    let t = parseSingleTransaction "Transaction,Adam,Dave,100" 1
    Assert.True(t.IsSome)
    Assert.Equal("Adam", t.Value.from)
    Assert.Equal("Dave", t.Value.``to``)
    Assert.Equal(100, t.Value.amount)

[<Fact>]
let ``apply from transaction``()=
    let miner = {name= "Adam"; balance=300; coinsMined = 0};
    let transaction = { from = "Adam"; ``to``= "Dave"; amount = 100 ; blockIndex = 1}
    Assert.Equal(200, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply to transaction``()=
    let miner = {name= "Adam"; balance=300;coinsMined = 0};
    let transaction = { from = "Dave"; ``to``= "Adam"; amount = 100 ; blockIndex = 1 }
    Assert.Equal(400, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply from and to transaction``()=
    let miner = {name= "Adam"; balance=300;coinsMined = 0};
    let transaction = { from = "Adam"; ``to``= "Adam"; amount = 100  ; blockIndex = 1}
    Assert.Equal(300, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply other transaction``()=
    let miner = {name= "Adam"; balance=300;coinsMined = 0};
    let transaction = { from = "Dave"; ``to``= "Fred"; amount = 100  ; blockIndex = 1}
    Assert.Equal(300, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply transactions to miners``()=
    let miners = [  { name = "Adam";balance = 100;coinsMined = 0};
                    { name = "Dave";balance = 200;coinsMined = 0}  ]
    let transaction = { from ="Adam"; ``to`` = "Dave"; amount = 30  ; blockIndex = 1}

    let newMiners = applyTransactionToMiners miners transaction |> Seq.toList
    
    Assert.Equal(2, newMiners.Length)
    Assert.Equal("Adam", newMiners.[0].name)
    Assert.Equal(70, newMiners.[0].balance)
    Assert.Equal("Dave", newMiners.[1].name)
    Assert.Equal(230, newMiners.[1].balance)

[<Fact>]
let ``apply transactions to miners when missing miner``()=
    let miners = [  ]
    let transaction = { from ="Adam"; ``to`` = "Dave"; amount = 30  ; blockIndex = 1}

    let newMiners = applyTransactionToMiners miners transaction |> Seq.toList
    
    Assert.Equal(2, newMiners.Length)
    Assert.Equal("Adam", newMiners.[0].name)
    Assert.Equal(-30, newMiners.[0].balance)
    Assert.Equal("Dave", newMiners.[1].name)
    Assert.Equal(30, newMiners.[1].balance)

[<Fact>]
let ``Get balancers from repository``()=
    let repository =   Repository.UnitTests.BlockDtoRepositoryTests.CreateRepositoryForTestDb()
    repository.DeleteAll()
    
    repository.Save(new BlockDto(1, "Adam", "Transaction,Adam,Dave,1", 0, "", ""));

    let miners = 
        getAllMiners(repository) 
        |> Seq.sortBy (fun m -> m.name)
        |> Seq.toList

    Assert.Equal(2, miners.Length)
    Assert.Equal("Adam", miners.[0].name)
    Assert.Equal(1, miners.[0].coinsMined)
    Assert.Equal(0, miners.[0].balance)
    Assert.Equal("Dave", miners.[1].name)
    Assert.Equal(0, miners.[1].coinsMined)
    Assert.Equal(1, miners.[1].balance)

    