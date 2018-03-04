#I @".\bin\Debug\netstandard2.0"
#I @".\bin\Debug\netstandard2.0"

#I @"C:\Users\Adam\.nuget\packages\mongodb.driver\2.5.0\lib\netstandard1.5\"
#I @"C:\Users\Adam\.nuget\packages\mongodb.bson\2.5.0\lib\netstandard1.5\"
#I @"C:\Users\Adam\.nuget\packages\mongodb.driver.core\2.5.0\lib\netstandard1.5\"

#r @"Repository.dll"
#r @"BlockChain.dll"

#r @"MongoDB.Driver.dll"
#r @"MongoDB.Bson.dll"
#r @"MongoDB.Driver.Core.dll"

open BlockChain.Miner
open BlockChain.MinerHelpers
open BlockChain

let data = 
    [
        "Transaction,Dave,Fred,1";
        "Transaction,Dave,Bob,2"
    ] 
    |> Seq.reduce (fun a b -> sprintf "%s%s%s" a ("\n") b )

//"5 Adam 4 0000CA2C984CDF70E03269309026979A18FA25EBBAB16BD90C88F92985992609 22889 0000CBFE51BE34FAD0DE5720D6253DD23556B909B2D9F1D412CF552CDB76EB91"
//|> parseBlock
//|> Miner.newBlock "Dave" data
//|> sprintBlock
//|> printf "%s"

let mongoUrl = "mongodb://localhost/BlockChain"
let repository = new Repository.BlockDtoRepository(mongoUrl)
let latestBlock = DtoHelpers.DtoToBlock(repository.TryGetLastBlock())

latestBlock
|> Miner.newBlock "Dave" data
|> sprintBlock  
|> printf "%s\n"

