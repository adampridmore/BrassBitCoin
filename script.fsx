open System
open System.Security.Cryptography
open System.Numerics;

//type Nonce = Int64

type Block = {
    index :int64
    data :  string
    previousHash : string
    nonce: int64
    hashText : string
}

let hash (content : String) = 
    let bytes = 
        content 
        |> System.Text.ASCIIEncoding.UTF8.GetBytes 
        |> HashAlgorithm.Create("SHA256").ComputeHash
    (BigInteger(Array.concat [ bytes ; [|0x0uy|]]), BitConverter.ToString( bytes ).Replace("-", "") )

let blockHash (i:int64) data (nonce:int64) (previousBlockHash: String)  = 
    [i |> string; data;previousBlockHash;nonce |> string]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hash

let isValidHash hash previousBlock = 
    printfn "%A %A" hash previousBlock.index;
    hash % ( BigInteger (previousBlock.index) ) = BigInteger.Zero

let newBlock data (previousBlock: Block) = 
    let nonce, _, hashText = 
        seq{0L..100L}
        |> Seq.map(fun nonce -> 
            let hashValue , hashText = (blockHash (previousBlock.index + 1L) data nonce previousBlock.hashText)
            
            nonce, hashValue, hashText)
        //|> Seq.map (fun (a, b, c) -> printfn "%A" b; (a,b,c))
        // This where / hash value check doesn't seem to work?
        |> Seq.where (fun (_, hash, _) -> isValidHash hash previousBlock )
        |> Seq.head
            
    {
        index = previousBlock.index + 1L;
        data = data;
        nonce = nonce;
        previousHash = previousBlock.hashText;
        hashText = hashText
    }
  
let genesisBlock =
    let index = 1L;
    let data = "Genesis";
    let previousHash = "0";
    let nonce = 0L;
    {
        index = index
        data = data
        previousHash = previousHash
        nonce = nonce
        hashText = (blockHash 0L data 0L previousHash |> snd)
    }

let tuple a = (a, a)

let blockchain =
    ["a";"b"]
    |> List.mapFold (fun previousBlock data -> previousBlock |> newBlock data |> tuple ) genesisBlock
    |> fst

[genesisBlock] @ blockchain


