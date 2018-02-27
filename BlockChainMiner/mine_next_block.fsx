#load "block.fs"
#load "miner.fs"

//#reference @"..\Repository\bin\Debug\netcoreapp2.0\Repository.dll"
//#reference @"..\Repository\bin\Debug\netcoreapp2.0\BlockChain.dll"


open BlockChain.Types
open BlockChain.Miner
open BlockChain

//open BlockChain
//open Repository


//let repository = Repository.BlockChainRepository("mongodb://...");
//let lastBlock = repository.TryGetLastBlock();

//BlockChain.Helpers.BlockHelpers.DtoToBlock(lastBlock )
//|> Miner.blockchain 1 
//|> Seq.exactlyOne


{
    block = 
        {
            index= 9L
            minedBy="Adam"
            data= "0"
            previousHash= "00002D93E57961F3D269E12ABFBAAA21A22B9A34C907AEFC94C6CF362DC07562"
            nonce = 60607L
        }
    hash= "000066FFAD8BC368DACDF4A17EFED490BC36B60A4B7A2E522DCE0A3179F6DC5D"
}
|> Miner.blockchain 1 "Dave"
|> Seq.exactlyOne

