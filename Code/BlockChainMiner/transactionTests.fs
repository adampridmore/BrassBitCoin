module transactionTests
open Xunit
open BlockChain.Types
open BlockChain.Transaction

[<Fact>]
let ``transaction to string``() =
    let txt =
        { from = "Adam"; ``to``= "Dave"; ammount = 100 }
        |> transactionToString 
    
    Assert.Equal("Transaction,Adam,Dave,100", txt)

[<Fact>]
let ``parse transaction``() =
    let t = parseTransaction "Transaction,Adam,Dave,100"
    Assert.True(t.IsSome)
    Assert.Equal("Adam", t.Value.from)
    Assert.Equal("Dave", t.Value.``to``)
    Assert.Equal(100, t.Value.ammount)

[<Fact>]
let ``apply from transaction``()=
    let miner = {name= "Adam"; balance=300};
    let transaction = { from = "Adam"; ``to``= "Dave"; ammount = 100 }
    Assert.Equal(200, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply to transaction``()=
    let miner = {name= "Adam"; balance=300};
    let transaction = { from = "Dave"; ``to``= "Adam"; ammount = 100 }
    Assert.Equal(400, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply from and to transaction``()=
    let miner = {name= "Adam"; balance=300};
    let transaction = { from = "Adam"; ``to``= "Adam"; ammount = 100 }
    Assert.Equal(300, (transaction |> applyTransaction miner).balance)

[<Fact>]
let ``apply other transaction``()=
    let miner = {name= "Adam"; balance=300};
    let transaction = { from = "Dave"; ``to``= "Fred"; ammount = 100 }
    Assert.Equal(300, (transaction |> applyTransaction miner).balance)
