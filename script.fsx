open System
open System.Security.Cryptography

type Block = {
    index :int64;
    timestamp : System.DateTime;
    data :  string
    previousHash : string
    hash : string
}

let blockHash (i:int64) (ts : System.DateTime)  data (previousBlockHash: String)  = 
    let hash (content : String) = 
        let bytes = 
            content 
            |> System.Text.ASCIIEncoding.UTF8.GetBytes 
            |> HashAlgorithm.Create("SHA1").ComputeHash

        BitConverter.ToString(bytes).Replace("-", "")
    
    [i |> string; ts.ToString("O");data;previousBlockHash]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hash

let newBlock (ts : System.DateTime) data (previousBlock: Block) = 
    {
        index = previousBlock.index + 1L;
        timestamp = ts;
        data = data;
        previousHash = previousBlock.hash;
        hash = blockHash (previousBlock.index + 1L) ts data previousBlock.hash
    }
  
let genesisBlock =
    {
        index = 0L;
        timestamp = new System.DateTime(2018,1,1,0,0,0,System.DateTimeKind.Utc);
        data = "Genesis";
        previousHash = "0000";
        hash = blockHash 0L (System.DateTime(2018,1,1,0,0,0,System.DateTimeKind.Utc)) "Genesis" "0000"
    }

genesisBlock
|> newBlock System.DateTime.UtcNow "My data" 
|> newBlock System.DateTime.UtcNow "My more data" 

