open System
open System.Security.Cryptography

type Block = {
    index :int64;
    data :  string
    previousHash : string
    hash : string
}

let blockHash (i:int64) data (previousBlockHash: String)  = 
    let hash (content : String) = 
        let bytes = 
            content 
            |> System.Text.ASCIIEncoding.UTF8.GetBytes 
            |> HashAlgorithm.Create("SHA1").ComputeHash

        BitConverter.ToString(bytes).Replace("-", "")
    
    [i |> string; data;previousBlockHash]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hash

let newBlock data (previousBlock: Block) = 
    {
        index = previousBlock.index + 1L;
        data = data;
        previousHash = previousBlock.hash;
        hash = blockHash (previousBlock.index + 1L) data previousBlock.hash
    }
  
let genesisBlock =
    {
        index = 0L;
        data = "Genesis";
        previousHash = "0000";
        hash = blockHash 0L "Genesis" "0000"
    }

genesisBlock
|> newBlock "My data" 
|> newBlock "My more data" 
|> newBlock "yet more data" 

