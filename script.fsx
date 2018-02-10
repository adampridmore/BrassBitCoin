open System
open System.Security.Cryptography
open System.Numerics;

type Block = {
    index :int64
    minedBy: string
    data :  string
    previousHash : string
    nonce: int64
}

type BlockWithHash = {
    block: Block;
    hashText: string
}

let computeHash (b: byte[]) = b |> HashAlgorithm.Create("SHA256").ComputeHash
let toPositiveBigInteger bytes = BigInteger(Array.concat [ bytes ; [|0x0uy|]])

let hash (content : String) = 
    let bytes = content |> System.Text.ASCIIEncoding.UTF8.GetBytes |> computeHash
    (bytes |> toPositiveBigInteger), ( BitConverter.ToString( bytes ).Replace("-", "") )

let blockHash (block: Block) = 
    [block.index |> string; block.minedBy; block.data; block.previousHash; block.nonce |> string]
    |> Seq.reduce (fun a b -> sprintf "%s %s" a b)
    |> hash

let isValidHash hash (previousBlock: BlockWithHash) = hash % ( BigInteger (previousBlock.block.index + 1L) ) = BigInteger.Zero

let newBlock minedBy data (previousBlock: BlockWithHash) = 
    let nonce, _, hashText = 
        Seq.initInfinite (fun i -> i |> int64)
        |> Seq.map(fun nonce -> 
            let block = {
                index = (previousBlock.block.index + 1L)
                minedBy = minedBy
                data = data
                nonce = nonce
                previousHash = previousBlock.hashText
            }

            let hashValue , hashText =  block |> blockHash
            nonce, hashValue, hashText)
        |> Seq.where (fun (_, hash, _) -> isValidHash hash previousBlock )
        |> Seq.head
            
    let block = {
        index = previousBlock.block.index + 1L;
        minedBy = minedBy;
        data = data;
        nonce = nonce;
        previousHash = previousBlock.hashText;
    }

    {
        block = block;
        hashText = hashText
    }
  
let genesisBlock =
    let block = {
        index = 1L;
        minedBy = "Genesis"
        data = "Genesis";
        previousHash = "0";
        nonce = 0L;
    }

    { 
        block = block
        hashText = (blockHash block) |> snd
    }

let tuple a = (a, a)

// let blockchain =
//     ["a";"b"]
//     |> List.mapFold (fun previousBlock data -> previousBlock |> newBlock data |> tuple ) genesisBlock
//     |> fst

// [genesisBlock] @ blockchain

let print x = x |> printfn "%A";x
let numbeOfBlocksToGenerate = 50
let blockchain2 =
    Seq.initInfinite id
    |> Seq.take numbeOfBlocksToGenerate
    |> Seq.map string
    |> Seq.toList
    |> Seq.mapFold (fun previousBlock data -> previousBlock |> newBlock "Adam" data |> print |> tuple ) genesisBlock
    |> fst
    |> Seq.toList

//#time "on"
blockchain2
|> Seq.iter (printfn "%A")


let init = ([1;2;3] |> List.toSeq )

let fib init =
    Seq.concat [init;
        Seq.unfold (fun state -> 
            let next = state |> Seq.sum
            Some(next, Seq.append (state |> Seq.skip(1)) [next] ) ) init
    ]
          
   
init 
|> fib
|> Seq.take 10

